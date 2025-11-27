using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }
    public const int LEVELS = 5; // asegúrate que coincide
    [Header("Llaves por nivel")]
    private bool[] keyCollected = new bool[LEVELS];

    public bool IsKeyCollected(int level)
    {
        if (level < 0 || level >= keyCollected.Length) return false;
        return keyCollected[level];
    }     
    public void SetKeyCollected(int level, bool value)
    {
        if (level < 0 || level >= keyCollected.Length) return;
        keyCollected[level] = value;
        // 🔔 Si tienes HUD, lo refresca de inmediato:
        if (FindFirstObjectByType<HUDController>() is HUDController hud)
            hud.FlashGold(0.3f);
    }
    [Header("Refs")]
    public GridManager grid;          // arrastra el GridManager (mismo objeto)
    public PlayerController player;   // arrastra el Player

    [Header("Stats")]
    public int HP = 3;                // vidas (fuerza de voluntad)
    public int Energy = 4;            // energía actual
    public int MaxEnergy = 4;
    public int LevelIndex = 0;        // 0..2
    public int TreasuresCollected = 0;
    public int Pasti = 0;

    [Header("Tiempo")]
    public float TimeLeft = 300f;     // 300 s
    public bool Running = true;

    [Header("Turnos")]
    public int QuietTurns = 0;        // turnos sin incidente
    public bool LastTurnHadIncident = false;

    [Header("Puntuación")]
    public const int STEP_POINTS = 10;
    public const int TREASURE_POINTS = 5000;
    public const int COIN_POINT_PER_PASTI = 5;
    public int Score = 0;
    [Header("UI/Banner")]
    public MessageBanner banner;

    [Header("SFX")]
    public AudioSource sfx;
    public AudioClip sfxTreasure;
    public bool HasAllTreasures => TreasuresCollected >= 5;

    
    void Awake()
    {
        I = this;
        for (int i = 0; i < LEVELS; i++)
            keyCollected[i] = false;
    }


    void Update()
{
    if (!Running) return;

    TimeLeft -= Time.deltaTime;
    if (TimeLeft < 0f) TimeLeft = 0f; 
}


    // ==== Reglas de energía/vidas ====
    public void OnWallBump()
{
    Energy = Mathf.Max(0, Energy - 1);
    FlagIncident();
    banner?.Show("¡Chocaste con una pared!", 0.8f);
    if (Energy == 0)
    {
        AddHp(-1);
        Energy = MaxEnergy;
    }
}

public void OnTrap()
{
    AddHp(-1);
    FlagIncident();
    banner?.Show("¡Trampa!", 0.8f);
}

    public void AddHp(int delta)
    {
        HP += delta;
        if (HP <= 0) GameOver(false);
    }

    // ==== Tesoros y niveles ====
   public void CollectTreasure()
    {
        TreasuresCollected++;

        // Mostrar mensaje con progreso y multiplicador actual
        int mul = TreasuresCollected switch { 0 => 1, 1 => 2, 2 => 4, _ => 6 };
        banner?.Show($"¡Tesoro {TreasuresCollected}/5!  Multiplicador x{mul}", 1.2f);

        // (No sumamos score aquí porque el tesoro solo afecta al multiplicador,
        // y tus puntos se aplican al subir de nivel con el tiempo restante)
        if (sfx && sfxTreasure) sfx.PlayOneShot(sfxTreasure);

    }

    public void NextLevelOrWin()
{
    // 1) Bono por TIEMPO RESTANTE del nivel actual
    int sec = Mathf.CeilToInt(TimeLeft);
    if (sec < 0) sec = 0;
    int timeBonus = sec * 100;
    AddScore(ApplyMultiplier(timeBonus));

    // 2) Bono por subir de nivel (tus valores)
    int[] levelBonus = { 10000, 15000, 20000, 25000, 30000 };
    if (LevelIndex < levelBonus.Length)
        AddScore(ApplyMultiplier(levelBonus[LevelIndex]));

    // 3) ¿Hay más niveles?
    // en GameManager.NextLevelOrWin()
if (LevelIndex < 4)  // <- cámbialo a 4 si ahora son 5 niveles (0..4)
{
    LevelIndex++;
    grid.currentLevel = LevelIndex;
    grid.RenderLevel(LevelIndex);

    var spawn = grid.GetSpawn(LevelIndex); // agrega este getter
    player.Init(grid, spawn, LevelIndex);
    grid.Reveal(spawn);
}
    else
    {
        // Último nivel: ganar
        GameOver(true);
    }
}

    // ==== Turnos (para +1 energía cada 3 turnos limpios) ====
    public void OnTurnEnded()
    {
        if (!LastTurnHadIncident)
        {
            QuietTurns++;
            if (QuietTurns >= 3)
            {
                QuietTurns = 0;
                Energy = Mathf.Min(MaxEnergy, Energy + 1);
            }
        }
        LastTurnHadIncident = false;
    }

    public void FlagIncident()
    {
        QuietTurns = 0;
        LastTurnHadIncident = true;
    }

    // ==== Pasti & Score ====
    public void AddPasti(int n) => Pasti += n;

    public void AddScore(int basePoints)
    {
        Score += basePoints;
        // (luego conectamos HUD)
    }

    // Multiplicador exacto: 0→×1, 1→×2, 2→×4, 3+→×6
    public int ApplyMultiplier(int pts)
    {
        int mul = TreasuresCollected switch { 0 => 1, 1 => 2, 2 => 4, _ => 6 };
        return pts * mul;
    }

    // ==== Fin de partida ====
    public void GameOver(bool win)
    {
        Running = false;

        if (win && TimeLeft > 0f)
        {
            int timeBonus = Mathf.RoundToInt(TimeLeft) * 100;
            AddScore(ApplyMultiplier(timeBonus));
        }

        // Mensaje y reinicio
        if (win)
        {
            banner?.Show("¡Ganaste! Reiniciando...", 2.0f);
            StartCoroutine(CoRestart(2.0f));
        }
        else
        {
            banner?.Show("Te moriste... Reiniciando", 2.0f);
            StartCoroutine(CoRestart(2.0f));
        }
    IEnumerator CoRestart(float delay)
    {
        Time.timeScale = 1f; // por si estabas en pausa/tienda
        yield return new WaitForSeconds(delay);
        // limpia singletons si hace falta
        I = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    }
    

    public void SaveGame()
{
    var d = new SaveData
    {
        hp = HP,
        energy = Energy,
        maxEnergy = MaxEnergy,
        levelIndex = LevelIndex,
        treasures = TreasuresCollected,
        pasti = Pasti,
        score = Score,
        timeLeft = TimeLeft,

        px = player.GridPos.x,
        py = player.GridPos.y,
        plevel = player.Level,

        map = grid.ExportMap(),
        visited = new System.Collections.Generic.List<string>()
    };

    // Visited (como ya hacías)
    foreach (var v in player.VisitedCells)
        d.visited.Add($"{v.x},{v.y}");

    // ⬇️ NUEVO: serializa llaves por nivel
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    for (int i = 0; i < LEVELS; i++)
        sb.Append(IsKeyCollected(i) ? '1' : '0');
    d.keys = sb.ToString();

    string json = JsonUtility.ToJson(d, false);
    SaveSystem.Save(json);
}
    public bool LoadGame()
{
    if (!SaveSystem.TryLoad(out var json)) return false;

    var d = JsonUtility.FromJson<SaveData>(json);
    if (d == null) return false;

    // Stats
    HP = d.hp;
    Energy = d.energy;
    MaxEnergy = d.maxEnergy;
    LevelIndex = d.levelIndex;
    TreasuresCollected = d.treasures;
    Pasti = d.pasti;
    Score = d.score;
    TimeLeft = d.timeLeft;

    // Map + nivel
    grid.currentLevel = d.levelIndex;
    grid.ImportMap(d.map);

    // Reposicionar jugador
    var spawn = new Vector2Int(Mathf.Clamp(d.px,0,GridManager.SIZE-1), Mathf.Clamp(d.py,0,GridManager.SIZE-1));
    player.Init(grid, spawn, d.plevel);

    // Revelar visitadas
    player.VisitedCells.Clear();
    foreach (var s in d.visited)
    {
        var parts = s.Split(',');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int x) &&
            int.TryParse(parts[1], out int y))
        {
            var p = new Vector2Int(x, y);
            grid.Reveal(p);
            player.VisitedCells.Add(p);
        }
    }

    // ⬇️ NUEVO: llaves por nivel
    if (!string.IsNullOrEmpty(d.keys))
    {
        for (int i = 0; i < Mathf.Min(d.keys.Length, LEVELS); i++)
        {
            bool has = d.keys[i] == '1';
            SetKeyCollected(i, has);
        }
    }
    else
    {
        // si no venía el campo (partidas viejas), deja todo en false
        for (int i = 0; i < LEVELS; i++) SetKeyCollected(i, false);
    }

    Running = true;
    return true;
}
}
