#include <windows.h>
#include <conio.h>
#include <deque>
#include <vector>
#include <random>
#include <chrono>
#include <thread>
#include <iostream>
#include <algorithm>
#include <sstream>
#include <string>

using namespace std;

struct Cell { int x, y; };
enum class Dir { Up, Down, Left, Right };

struct GameConfig {
    int cols = 40;         // ancho útil (en celdas lógicas)
    int rows = 20;         // alto útil (en celdas lógicas)
    int tick_ms = 100;     // ms por paso (reduce para más velocidad)
    bool wallsKill = true; // true: chocar con muro muere; false: mundo toroidal
};

// Conversión numérica a string (sin depender de C++11)
template<typename T>
string numToString(const T& v) { ostringstream oss; oss << v; return oss.str(); }

class SnakeGame {
public:
    SnakeGame(const GameConfig& cfg) : C(cfg.cols), R(cfg.rows), tick(cfg.tick_ms), wallsKill(cfg.wallsKill) {
        hOut = GetStdHandle(STD_OUTPUT_HANDLE);
        hideCursor();
        reset();
    }

    void run() {
        using clock = std::chrono::steady_clock;
        auto next = clock::now();

        while (running) {
            handleInput();

            auto now = clock::now();
            if (alive && now >= next) {
                step();
                next = now + std::chrono::milliseconds(tick); // paso fijo
            }

            draw();
            std::this_thread::sleep_for(std::chrono::milliseconds(alive ? 1 : 16));
        }
    }

private:
    // --- Config lógica ---
    int C, R;               // área jugable (celdas)
    int tick;               // ms por paso
    bool wallsKill;

    // --- Estado ---
    std::deque<Cell> snake; // [0] = cabeza
    Dir dir;
    Cell food;
    bool alive = true;
    bool running = true;
    int score = 0;

    // --- IO ---
    HANDLE hOut;
    std::mt19937 rng{ std::random_device{}() };

    void reset() {
        snake.clear();
        int cx = C / 2, cy = R / 2;
        snake.push_front({ cx, cy });
        snake.push_back({ cx - 1, cy });
        snake.push_back({ cx - 2, cy });
        dir = Dir::Right;
        score = 0;
        alive = true;
        placeFood();
        clearScreen();
    }

    void placeFood() {
        std::uniform_int_distribution<int> dx(0, C - 1), dy(0, R - 1);
        while (true) {
            Cell f{ dx(rng), dy(rng) };
            bool onSnake = false;
            for (auto& s : snake) if (s.x == f.x && s.y == f.y) { onSnake = true; break; }
            if (!onSnake) { food = f; return; }
        }
    }

    void handleInput() {
        if (!_kbhit()) return;
        int c = _getch();
        // flechas (224 + código)
        if (c == 224 && _kbhit()) {
            int a = _getch();
            if (a == 72 && dir != Dir::Down)  dir = Dir::Up;    // Up
            if (a == 80 && dir != Dir::Up)    dir = Dir::Down;  // Down
            if (a == 75 && dir != Dir::Right) dir = Dir::Left;  // Left
            if (a == 77 && dir != Dir::Left)  dir = Dir::Right; // Right
            return;
        }
        // WASD + Esc
        if ((c == 'w' || c == 'W') && dir != Dir::Down)  dir = Dir::Up;
        else if ((c == 's' || c == 'S') && dir != Dir::Up)    dir = Dir::Down;
        else if ((c == 'a' || c == 'A') && dir != Dir::Right) dir = Dir::Left;
        else if ((c == 'd' || c == 'D') && dir != Dir::Left)  dir = Dir::Right;
        else if (c == 27) running = false;          // Esc
        else if ((c == 'r' || c == 'R') && !alive) reset(); // Reiniciar si está muerto
    }

    void step() {
        Cell head = snake.front();
        if (dir == Dir::Up)    head.y--;
        if (dir == Dir::Down)  head.y++;
        if (dir == Dir::Left)  head.x--;
        if (dir == Dir::Right) head.x++;

        if (wallsKill) {
            if (head.x < 0 || head.x >= C || head.y < 0 || head.y >= R) { alive = false; return; }
        }
        else {
            if (head.x < 0) head.x = C - 1;
            if (head.x >= C) head.x = 0;
            if (head.y < 0) head.y = R - 1;
            if (head.y >= R) head.y = 0;
        }

        for (auto& s : snake) if (s.x == head.x && s.y == head.y) { alive = false; return; }

        snake.push_front(head);
        if (head.x == food.x && head.y == food.y) {
            score += 10;
            placeFood();
            if (tick > 40 && (score % 50 == 0)) tick -= 5; // acelera ligeramente
        }
        else {
            snake.pop_back();
        }
    }

    void draw() {
        // --- CELDA DOBLE ANCHO para cuadrar aspecto ---
        const int CELL_W = 2;                       // 2 caracteres por celda
        const string WALL_H = string(C * CELL_W, '-');
        const string SPACE2 = string(CELL_W, ' ');

        // ancho y alto del buffer de texto
        const int W = C * CELL_W + 2;               // +2 por el marco lateral
        const int H = R + 4;                        // +2 marco +2 HUD

        string buf;
        buf.reserve(W * H + H);

        // HUD
        buf += "  Snake (C++/WinCon)   Puntos: " + numToString(score) + "\n";

        // marco superior
        buf += "+" + WALL_H + "+\n";

        // área jugable
        for (int y = 0; y < R; ++y) {
            buf += "|";
            for (int x = 0; x < C; ++x) {
                bool isFood = (x == food.x && y == food.y);
                // ¿está la serpiente?
                int idx = -1;
                for (size_t i = 0; i < snake.size(); ++i) {
                    if (snake[i].x == x && snake[i].y == y) { idx = (int)i; break; }
                }

                if (idx == 0) {
                    // cabeza: dos caracteres
                    buf += "OO";
                }
                else if (idx > 0) {
                    // cuerpo: dos caracteres
                    buf += "oo";
                }
                else if (isFood) {
                    // comida: dos caracteres
                    buf += "**";
                }
                else {
                    buf += SPACE2; // celda vacía
                }
            }
            buf += "|\n";
        }

        // marco inferior
        buf += "+" + WALL_H + "+\n";

        if (!alive) buf += "  ** GAME OVER **  [R] Reiniciar   [Esc] Salir\n";
        else        buf += "  Flechas/WASD para mover. [Esc] Salir\n";

        // volcar buffer
        COORD c{ 0,0 };
        SetConsoleCursorPosition(hOut, c);
        DWORD written = 0;
        WriteConsoleA(hOut, buf.c_str(), (DWORD)buf.size(), &written, nullptr);
    }

    void hideCursor() {
        CONSOLE_CURSOR_INFO info{}; info.dwSize = 100; info.bVisible = FALSE;
        SetConsoleCursorInfo(hOut, &info);
    }

    void clearScreen() {
        CONSOLE_SCREEN_BUFFER_INFO csbi;
        GetConsoleScreenBufferInfo(hOut, &csbi);
        DWORD size = csbi.dwSize.X * csbi.dwSize.Y, written;
        FillConsoleOutputCharacter(hOut, ' ', size, { 0,0 }, &written);
        FillConsoleOutputAttribute(hOut, csbi.wAttributes, size, { 0,0 }, &written);
        SetConsoleCursorPosition(hOut, { 0,0 });
    }
};

int main() {
    GameConfig cfg;
    // cfg.wallsKill = false; // mundo toroidal si lo quieres
    SnakeGame game(cfg);
    game.run();
    return 0;
}
