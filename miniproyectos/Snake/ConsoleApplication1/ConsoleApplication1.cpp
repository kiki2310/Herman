#define NOMINMAX
#include <windows.h>
#include <conio.h>
#include <chrono>
#include <thread>
#include <random>
#include <string>
#include <sstream>
#include <iostream>
#include <vector>
#include <algorithm>   // max, max_element
#include <fstream>
#include <limits>
#include <unordered_set> // <-- añadido

using namespace std;

// ===== Utils =====
template<typename T>
string numToString(const T& v) { ostringstream oss; oss << v; return oss.str(); }
string formatTime(double sec) { ostringstream oss; oss.setf(std::ios::fixed); oss.precision(1); oss << sec; return oss.str(); }

// ===== Lista enlazada =====
struct Cell { int x, y; };
struct Node { Cell c; Node* next; Node(Cell v) : c(v), next(nullptr) {} };

class SnakeList {
public:
    SnakeList() : head_(nullptr), tail_(nullptr), n_(0) {}
    ~SnakeList() { clear(); }
    void clear() { for (Node* p = head_;p;) { Node* q = p->next; delete p; p = q; } head_ = tail_ = nullptr; n_ = 0; }
    void push_front(Cell c) { Node* p = new Node(c); p->next = head_; head_ = p; if (!tail_) tail_ = p; ++n_; }
    void pop_back() {
        if (!head_) return;
        if (head_ == tail_) { delete head_; head_ = tail_ = nullptr; n_ = 0; return; }
        Node* p = head_; while (p->next != tail_) p = p->next;
        delete tail_; tail_ = p; tail_->next = nullptr; --n_;
    }
    bool contains(int x, int y, const Node* exclude = nullptr) const {
        for (Node* p = head_;p;p = p->next) { if (p == exclude) continue; if (p->c.x == x && p->c.y == y) return true; } return false;
    }
    Node* head() const { return head_; }
    Node* tail() const { return tail_; }
    Cell headCell() const { return head_->c; }
    size_t size() const { return n_; }
private:
    Node* head_; Node* tail_; size_t n_;
};

// ===== Juego =====
enum class Dir { Up, Down, Left, Right };

struct GameConfig { int cols = 20; int rows = 20; int tick_ms = 120; bool toroidal = true; };

struct ScoreEntry { string name; int score; int levels; double timeSec; };

class SnakeGame {
public:
    SnakeGame(const GameConfig& cfg)
        : C(cfg.cols), R(cfg.rows), tick(cfg.tick_ms), baseTick(cfg.tick_ms), toroidal(cfg.toroidal)
    {
        hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleScreenBufferInfo(hOut, &csbiDefault);
        showConsoleCursor(false);
        rng.seed(random_device{}());
        loadScores(); recomputeBestFromTable();
        promptPlayerName();
        resetAllForNewRun();
        setupBackBuffer();            // define W/H del contenido
        refreshScreenSizeAndBuffer(); // mide pantalla y centra
    }

    void run() {
        using Clock = std::chrono::steady_clock;
        auto nextStep = Clock::now();
        while (running) {
            if (!awaitingRanking) handleInput();

            auto now = Clock::now();
            if (alive && !paused && !awaitingRanking && now >= nextStep) {
                applyQueuedTurn();
                step();
                nextStep = now + std::chrono::milliseconds(tick);
            }

            if (awaitingRanking) {
                registerRankingAndPrompt();
                resetAllForNewRun();
                nextStep = Clock::now() + std::chrono::milliseconds(tick);
                continue;
            }

            drawFrame();
            std::this_thread::sleep_for(std::chrono::milliseconds(paused ? 16 : 1));
        }
        SetConsoleTextAttribute(hOut, csbiDefault.wAttributes);
    }

private:
    // ----- Config contenido -----
    const int C, R;            // celdas del tablero (20x20)
    int tick, baseTick;       // ms/paso
    const bool toroidal;

    // estado general
    bool running = true, paused = false, alive = true, awaitingRanking = false;

    // puntuación / nivel / tiempo
    int score = 0, level = 1;
    std::chrono::steady_clock::time_point startTime;
    double accumulatedTimeSec = 0.0, showTimeSec = 0.0;

    // jugador
    string playerName = "PLAYER";

    // serpiente
    SnakeList snake;
    Dir dir = Dir::Right, queuedDir = Dir::Right;
    bool hasQueued = false;

    // items
    Cell food{ 0,0 }, trap{ 0,0 };

    // render (contenido) y consola (pantalla)
    HANDLE hOut; CONSOLE_SCREEN_BUFFER_INFO csbiDefault;
    int CELL_W = 2;    // ancho de cada celda en caracteres (2 para doble ancho; auto-baja a 1 si no cabe)
    int W = 0, H = 0;    // tamaño del contenido (HUD + tablero + ayuda)
    // tamaño visible de consola y offsets para centrar:
    int SW = 0, SH = 0;  // Screen Width/Height visibles
    int OX = 0, OY = 0;  // offsets para centrar el contenido dentro de la pantalla
    vector<CHAR_INFO> buffer; // tamaño = SW*SH

    // ranking
    vector<ScoreEntry> table;
    const char* SCORE_FILE = "snake_scores.txt";

    // guardado
    const char* SAVE_FILE = "snake_save.txt";

    // RNG
    mt19937 rng; unsigned int rngSeed = 0;

    // modo/meta
    bool hard = false;
    int accelNormal = 10, accelHard = 15;
    ScoreEntry bestAllTime{ "-",0,0,0.0 };

    // ===== Colores / helpers =====
    static WORD F_WHITE() { return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE | FOREGROUND_INTENSITY; }
    static WORD F_GRAY() { return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE; }
    static WORD F_GREEN() { return FOREGROUND_GREEN | FOREGROUND_INTENSITY; }
    static WORD F_DKGREEN() { return FOREGROUND_GREEN; }
    static WORD F_RED() { return FOREGROUND_RED | FOREGROUND_INTENSITY; }
    static WORD F_YELLOW() { return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_INTENSITY; }
    static CHAR_INFO makeCI(char ch, WORD a) { CHAR_INFO ci{}; ci.Char.AsciiChar = ch; ci.Attributes = a; return ci; }

    void setupBackBuffer() { W = C * CELL_W + 2; H = R + 5; /*HUD(1)+marcos(2)+ayuda(1)+1 margen*/ }

    // --- helpers pantalla centrada ---
    void refreshScreenSizeAndBuffer() {
        CONSOLE_SCREEN_BUFFER_INFO csbi{};
        GetConsoleScreenBufferInfo(hOut, &csbi);
        SW = csbi.srWindow.Right - csbi.srWindow.Left + 1;
        SH = csbi.srWindow.Bottom - csbi.srWindow.Top + 1;
        if (SW <= 0) SW = W; if (SH <= 0) SH = H;
        OX = (SW > W) ? (SW - W) / 2 : 0;
        OY = (SH > H) ? (SH - H) / 2 : 0;
        buffer.assign(SW * SH, makeCI(' ', F_GRAY()));
    }
    inline int GX(int x) const { return OX + x; }
    inline int GY(int y) const { return OY + y; }
    void putAbs(int x, int y, char ch, WORD a) { if (x < 0 || y < 0 || x >= SW || y >= SH) return; auto& ci = buffer[y * SW + x]; ci.Char.AsciiChar = ch; ci.Attributes = a; }
    void putsAbsClipped(int x, int y, string s, WORD a) {
        if (y < 0 || y >= SH) return;
        if (x < 0) { if ((int)s.size() + x <= 0) return; s = s.substr(-x); x = 0; }
        if (x + (int)s.size() > SW) s = s.substr(0, max(0, SW - x));
        for (size_t i = 0;i < s.size();++i) putAbs(x + (int)i, y, s[i], a);
    }
    int centerX(const string& s) const { return max(0, (SW - (int)s.size()) / 2); }
    void putsCentered(int y, const string& s, WORD a) { putsAbsClipped(centerX(s), y, s, a); }

    // ===== Ranking =====
    void loadScores() {
        table.clear(); ifstream in(SCORE_FILE); if (!in.good()) return;
        string line;
        while (getline(in, line)) {
            if (line.empty()) continue;
            vector<string> p; size_t pos = 0, prev = 0;
            while ((pos = line.find('\t', prev)) != string::npos) { p.push_back(line.substr(prev, pos - prev)); prev = pos + 1; }
            p.push_back(line.substr(prev));
            if (p.size() < 4) continue;
            table.push_back({ p[0], atoi(p[1].c_str()), atoi(p[2].c_str()), atof(p[3].c_str()) });
        }
        sortScores(); trimScores(10);
    }
    void saveScores() { ofstream out(SCORE_FILE, ios::trunc); for (auto& e : table) out << e.name << "\t" << e.score << "\t" << e.levels << "\t" << e.timeSec << "\n"; }
    void sortScores() { sort(table.begin(), table.end(), [](auto& a, auto& b) { if (a.score != b.score) return a.score > b.score; if (a.levels != b.levels) return a.levels > b.levels; return a.timeSec < b.timeSec;}); }
    void trimScores(size_t n) { if (table.size() > n) table.resize(n); }
    void addScore(const string& name, int s, int lv, double t) { table.push_back({ name,s,lv,t }); sortScores(); trimScores(10); saveScores(); recomputeBestFromTable(); }
    void recomputeBestFromTable() {
        if (table.empty()) { bestAllTime = { "-",0,0,0.0 }; return; }
        auto better = [](const ScoreEntry& a, const ScoreEntry& b) { if (a.score != b.score) return a.score > b.score; if (a.levels != b.levels) return a.levels > b.levels; return a.timeSec < b.timeSec; };
        bestAllTime = *max_element(table.begin(), table.end(), [&](auto& x, auto& y) {return better(y, x);});
    }

    // ===== Tiempo =====
    double effectiveElapsedSec() const {
        auto now = std::chrono::steady_clock::now();
        double runningSec = std::chrono::duration_cast<std::chrono::milliseconds>(now - startTime).count() / 1000.0;
        return accumulatedTimeSec + (alive && !awaitingRanking ? runningSec : 0.0);
    }

    // ===== Save/Load =====
    void saveGame() {
        ofstream out(SAVE_FILE, ios::trunc); if (!out.good()) return;
        out << playerName << "\n";
        double toSaveTime = effectiveElapsedSec();
        out << C << " " << R << " " << tick << " " << baseTick << " " << level << " " << score << " " << toSaveTime << "\n";
        out << (int)dir << "\n";
        out << food.x << " " << food.y << "\n" << trap.x << " " << trap.y << "\n";
        out << snake.size() << "\n";
        vector<Cell> rev; rev.reserve(snake.size());
        for (Node* p = snake.head();p;p = p->next) rev.push_back(p->c);
        for (auto& c : rev) out << c.x << " " << c.y << "\n";
        out << rngSeed << "\n";
    }
    bool loadGame() {
        ifstream in(SAVE_FILE); if (!in.good()) return false;
        getline(in, playerName);
        int c, r, t, bT, lv, sc; double acc; int idir;
        in >> c >> r >> t >> bT >> lv >> sc >> acc; in.ignore(numeric_limits<streamsize>::max(), '\n');
        if (c != C || r != R) return false;
        tick = t; baseTick = bT; level = lv; score = sc; accumulatedTimeSec = acc;
        in >> idir; in.ignore(numeric_limits<streamsize>::max(), '\n'); dir = (Dir)idir; queuedDir = dir; hasQueued = false;
        in >> food.x >> food.y; in.ignore(numeric_limits<streamsize>::max(), '\n');
        in >> trap.x >> trap.y; in.ignore(numeric_limits<streamsize>::max(), '\n');
        int n; in >> n; in.ignore(numeric_limits<streamsize>::max(), '\n');
        SnakeList tmp; for (int i = 0;i < n;i++) { int x, y; in >> x >> y; in.ignore(numeric_limits<streamsize>::max(), '\n'); tmp.push_front({ x,y }); }
        vector<Cell> cells; for (Node* p = tmp.head();p;p = p->next) cells.push_back(p->c);
        snake.clear(); for (int i = (int)cells.size() - 1;i >= 0;--i) snake.push_front(cells[i]);
        in >> rngSeed; if (rngSeed == 0) rngSeed = (unsigned)random_device{}(); rng.seed(rngSeed);
        startTime = std::chrono::steady_clock::now(); alive = true; paused = false; awaitingRanking = false;
        return true;
    }

    // ===== Flujo =====
    void promptPlayerName() {
        system("cls"); SetConsoleTextAttribute(hOut, F_WHITE());
        cout << "\n\n   SNAKE 20x20 (Lista Enlazada)\n\n";
        cout << "   Ingresa tu nombre (Enter para 'PLAYER'): ";
        string name; getline(cin, name); if (name.empty()) name = "PLAYER"; if (name.size() > 20) name = name.substr(0, 20);
        name.erase(remove(name.begin(), name.end(), '\t'), name.end()); playerName = name;
    }
    void resetAllForNewRun() {
        score = 0; level = 1; tick = baseTick; accumulatedTimeSec = 0.0;
        newLevelSetup(true);
        rngSeed = (unsigned)random_device{}(); rng.seed(rngSeed);
    }
    void newLevelSetup(bool) {
        snake.clear();
        int cx = C / 2, cy = R / 2;
        for (int i = 4;i >= 0;--i) snake.push_front({ cx - i, cy }); // tamaño inicial 5
        dir = Dir::Right; queuedDir = dir; hasQueued = false;
        startTime = std::chrono::steady_clock::now(); showTimeSec = accumulatedTimeSec;
        placeFoodAvoidAll(); placeTrapAvoidAll();
        clearScreen(); alive = true; paused = false; awaitingRanking = false;
    }

    // ===== Input =====
    static bool isOpposite(Dir a, Dir b) {
        return (a == Dir::Up && b == Dir::Down) || (a == Dir::Down && b == Dir::Up) || (a == Dir::Left && b == Dir::Right) || (a == Dir::Right && b == Dir::Left);
    }
    void queueTurn(Dir d) { if (!isOpposite(dir, d)) { queuedDir = d; hasQueued = true; } }
    void applyQueuedTurn() { if (hasQueued) { dir = queuedDir; hasQueued = false; } }

    void handleInput() {
        while (_kbhit()) {
            int c = _getch();
            if (c == 224 && _kbhit()) {
                int a = _getch();
                if (a == 72) queueTurn(Dir::Up); if (a == 80) queueTurn(Dir::Down); if (a == 75) queueTurn(Dir::Left); if (a == 77) queueTurn(Dir::Right); continue;
            }
            if (c == 'w' || c == 'W') queueTurn(Dir::Up);
            else if (c == 's' || c == 'S') queueTurn(Dir::Down);
            else if (c == 'a' || c == 'A') queueTurn(Dir::Left);
            else if (c == 'd' || c == 'D') queueTurn(Dir::Right);
            else if (c == 'p' || c == 'P') paused = !paused;
            else if (c == 'r' || c == 'R') newLevelSetup(false);
            else if (c == 'g' || c == 'G') saveGame();
            else if (c == 'l' || c == 'L') loadGame();
            else if (c == 'h' || c == 'H') { hard = !hard; newLevelSetup(false); }
            else if (c == 27) running = false;
        }
    }

    // ===== Reglas =====
    int targetLen() const { return hard ? 20 : 15; }
    int accelPerLevel() const { return hard ? accelHard : accelNormal; }

    void placeFoodAvoidAll() {
        uniform_int_distribution<int> dx(0, C - 1), dy(0, R - 1);
        for (;;) { Cell f{ dx(rng),dy(rng) }; if (!snake.contains(f.x, f.y) && !(f.x == trap.x && f.y == trap.y)) { food = f; return; } }
    }
    void placeTrapAvoidAll() {
        uniform_int_distribution<int> dx(0, C - 1), dy(0, R - 1);
        for (;;) { Cell t{ dx(rng),dy(rng) }; if (!snake.contains(t.x, t.y) && !(t.x == food.x && t.y == food.y)) { trap = t; return; } }
    }
    void respawnBothItems() { placeFoodAvoidAll(); placeTrapAvoidAll(); }
    Cell wrapped(Cell h) { if (toroidal) { if (h.x < 0) h.x = C - 1; else if (h.x >= C) h.x = 0; if (h.y < 0) h.y = R - 1; else if (h.y >= R) h.y = 0; } return h; }

    // ---------- NUEVO: helpers de integridad ----------
    inline long long packXY(int x, int y) const {
        return ((static_cast<long long>(y) << 32) ^ static_cast<unsigned long long>(x));
    }
    bool neighbors4Toroidal(const Cell& a, const Cell& b) const {
        int dx = std::abs(a.x - b.x);
        int dy = std::abs(a.y - b.y);
        if (toroidal) {
            dx = std::min(dx, C - dx);
            dy = std::min(dy, R - dy);
        }
        return (dx + dy) == 1;
    }
    bool snakeIntegrityOK() const {
        if (!snake.head()) return false;

        std::unordered_set<long long> seen;
        seen.reserve(snake.size() * 2);

        for (const Node* q = snake.head(); q; q = q->next) {
            int x = q->c.x, y = q->c.y;

            if (!toroidal) { // en modo no toroidal, salir del tablero rompe estructura
                if (x < 0 || x >= C || y < 0 || y >= R) return false;
            }

            int xn = ((x % C) + C) % C;
            int yn = ((y % R) + R) % R;
            long long k = packXY(xn, yn);
            if (!seen.insert(k).second) return false; // duplicado

            if (q->next && !neighbors4Toroidal(q->c, q->next->c)) return false; // no contigua
        }
        return true;
    }
    // ---------- FIN NUEVO ----------

    void step() {
        // actualiza tiempo mostrado
        showTimeSec = effectiveElapsedSec();

        // calcular nueva cabeza
        Cell h = snake.headCell();
        if (dir == Dir::Up) --h.y;
        if (dir == Dir::Down) ++h.y;
        if (dir == Dir::Left) --h.x;
        if (dir == Dir::Right) ++h.x;
        h = wrapped(h); // respeta modo toroidal

        bool eat = (h.x == food.x && h.y == food.y);
        bool hitTrap = (h.x == trap.x && h.y == trap.y);

        // permitir “pasar sobre la cola” si no come (cola se moverá)
        const Node* excludeTail = (!eat ? snake.tail() : nullptr);
        if (snake.contains(h.x, h.y, excludeTail)) {
            accumulatedTimeSec = effectiveElapsedSec();
            alive = false; awaitingRanking = true; return;
        }

        // aplicar movimiento
        snake.push_front(h);
        if (eat) score += 100; else snake.pop_back();

        // trampa: penaliza y encoge
        if (hitTrap) {
            score -= 200;
            if (snake.size() > 1) snake.pop_back();
        }

        // reubicar items si se interactuó
        if (eat || hitTrap) respawnBothItems();

        // perder si la estructura se rompe (no contigua / duplicada / fuera en no-toroidal)
        if (!snakeIntegrityOK()) {
            accumulatedTimeSec = effectiveElapsedSec();
            alive = false; awaitingRanking = true; return;
        }

        // >>> NUEVO: perder si quedó con tamaño < 2 (solo cabeza = sin estructura)
        if ((int)snake.size() < 2) {
            accumulatedTimeSec = effectiveElapsedSec();
            alive = false; awaitingRanking = true; return;
        }
        // <<< FIN NUEVO

        // subir de nivel si se alcanza la meta de longitud
        if ((int)snake.size() >= targetLen()) {
            level += 1;
            score += 1000;
            tick = std::max(40, tick - accelPerLevel());
            accumulatedTimeSec = effectiveElapsedSec(); // cierra nivel
            newLevelSetup(false);                       // tamaño reinicia a 5
        }
    }


    // ===== Render (centrado y adaptativo) =====
    void drawFrame() {
        // Si la ventana es angosta, compacta a celdas de 1 para que quepa el tablero
        if (CELL_W == 2 && (C * CELL_W + 2) > SW) { CELL_W = 1; setupBackBuffer(); }

        // actualizar tamaño visible y recrear buffer centrado
        refreshScreenSizeAndBuffer();
        std::fill(buffer.begin(), buffer.end(), makeCI(' ', F_GRAY()));

        // HUD con 3 longitudes, centrado
        string hudFull = "  " + playerName +
            "  Pts:" + numToString(score) +
            "  Niv:" + numToString(level) +
            "  Tam:" + numToString((int)snake.size()) + "/" + numToString(targetLen()) +
            "  Vel(ms):" + numToString(tick) +
            "  T:" + formatTime(showTimeSec) + "s" +
            "  Best:" + numToString(bestAllTime.score) + " L" + numToString(bestAllTime.levels) +
            " " + formatTime(bestAllTime.timeSec) + "s" +
            "  Mode:" + string(hard ? "H" : "N") +
            (paused ? "  [PAUSA]" : "");

        string hudMid = " " + playerName +
            "  P:" + numToString(score) +
            "  L:" + numToString(level) +
            "  S:" + numToString((int)snake.size()) + "/" + numToString(targetLen()) +
            "  V:" + numToString(tick) +
            "  t:" + formatTime(showTimeSec) +
            (paused ? " [PAUSA]" : "");

        string hudMini = playerName + "  P:" + numToString(score) + "  L:" + numToString(level);

        auto pickThatFits = [&](const string& a, const string& b, const string& c)->string {
            if ((int)a.size() <= SW - 2) return a;
            if ((int)b.size() <= SW - 2) return b;
            return c;
            };
        string hud = pickThatFits(hudFull, hudMid, hudMini);
        putsCentered(GY(0), hud, F_WHITE());

        // Desplazamiento visual opcional para airear el tablero
        int extraTop = (SH >= H + 2) ? 1 : 0;

        // Marco superior
        putsAbsClipped(GX(0), GY(1 + extraTop), "+", F_WHITE());
        for (int i = 0;i < C * CELL_W;++i) putAbs(GX(1 + i), GY(1 + extraTop), '-', F_WHITE());
        putAbs(GX(1 + C * CELL_W), GY(1 + extraTop), '+', F_WHITE());

        // Área jugable
        for (int y = 0;y < R;++y) {
            int vy = GY(y + 2 + extraTop);
            putAbs(GX(0), vy, '|', F_WHITE());
            putAbs(GX(1 + C * CELL_W), vy, '|', F_WHITE());

            for (int x = 0;x < C;++x) {
                bool isFood = (x == food.x && y == food.y);
                bool isTrap = (x == trap.x && y == trap.y);

                bool isSnake = false, isHead = false;
                for (Node* p = snake.head(); p; p = p->next) {
                    if (p->c.x == x && p->c.y == y) { isSnake = true; isHead = (p == snake.head()); break; }
                }

                int vx = GX(1 + x * CELL_W);
                auto paint2 = [&](char ch, WORD a) {
                    putAbs(vx, vy, ch, a);
                    if (CELL_W == 2) putAbs(vx + 1, vy, ch, a);
                    };

                if (isHead)       paint2('O', F_GREEN());
                else if (isSnake) paint2('o', F_DKGREEN());
                else if (isFood)  paint2('*', F_RED());
                else if (isTrap)  paint2('X', F_YELLOW());
                else             paint2(' ', F_GRAY());
            }
        }

        // Marco inferior
        int fy = GY(R + 2 + extraTop);
        putsAbsClipped(GX(0), fy, "+", F_WHITE());
        for (int i = 0;i < C * CELL_W;++i) putAbs(GX(1 + i), fy, '-', F_WHITE());
        putAbs(GX(1 + C * CELL_W), fy, '+', F_WHITE());

        // Ayuda (3 longitudes), centrada
        if (fy + 1 < SH) {
            string helpFull = "  Flechas/WASD mover   [P] Pausa   [R] Reiniciar nivel   [H] Normal/Hard   [G] Guardar   [L] Cargar   [Esc] Salir";
            string helpMid = "  Flechas/WASD   [P] Pausa   [R] Reiniciar   [H] Hard   [G] Guardar   [L] Cargar   [Esc]";
            string helpMini = "  ←↑→↓/WASD  [P]  [R]  [H]  [G]  [L]  [Esc]";
            string help = pickThatFits(helpFull, helpMid, helpMini);
            putsCentered(fy + 1, help, F_WHITE());
        }

        // Volcar buffer del tamaño de pantalla
        SMALL_RECT rect{ 0,0,(SHORT)(SW - 1),(SHORT)(SH - 1) };
        COORD bufSize{ (SHORT)SW,(SHORT)SH };
        COORD bufCoord{ 0,0 };
        WriteConsoleOutputA(hOut, buffer.data(), bufSize, bufCoord, &rect);
    }

    // ===== Ranking (pantalla) =====
    void registerRankingAndPrompt() {
        system("cls"); SetConsoleTextAttribute(hOut, F_WHITE());
        cout << "\n\n   ** GAME OVER **\n\n";
        cout << "   Jugador: " << playerName << "\n";
        cout << "   Puntos : " << score << "\n";
        cout << "   Niveles: " << level << "\n";
        cout << "   Tiempo : " << formatTime(accumulatedTimeSec) << " s\n";
        cout << "   Modo   : " << (hard ? "Hard (meta 20)" : "Normal (meta 15)") << "\n\n";
        addScore(playerName, score, level, accumulatedTimeSec);

        cout << "   ====== TOP 10 ======\n\n";
        int pos = 1;
        for (auto& e : table) {
            cout << "   " << (pos < 10 ? " " : "") << pos << ". "
                << e.name << string(std::max(1, 18 - (int)e.name.size()), ' ')
                << "  " << e.score << " pts"
                << "  L" << e.levels
                << "  " << formatTime(e.timeSec) << " s\n";
            if (++pos > 10) break;
        }
        cout << "\n   [R] Nueva partida   [Esc] Salir\n";
        cout.flush();
        for (;;) { int c = _getch(); if (c == 27) { running = false; break; } if (c == 'r' || c == 'R') break; }
    }

    // ===== Consola =====
    void showConsoleCursor(bool show) {
        CONSOLE_CURSOR_INFO info{}; info.dwSize = 100; info.bVisible = show ? TRUE : FALSE;
        SetConsoleCursorInfo(hOut, &info);
    }
    void clearScreen() {
        CONSOLE_SCREEN_BUFFER_INFO csbi; GetConsoleScreenBufferInfo(hOut, &csbi);
        DWORD size = csbi.dwSize.X * csbi.dwSize.Y, written;
        FillConsoleOutputCharacter(hOut, ' ', size, { 0,0 }, &written);
        FillConsoleOutputAttribute(hOut, csbi.wAttributes, size, { 0,0 }, &written);
        SetConsoleCursorPosition(hOut, { 0,0 });
    }
};

int main() {
    GameConfig cfg; // 20x20, wrap ON, tick base 120 ms
    SnakeGame game(cfg);
    game.run();
    return 0;
}
