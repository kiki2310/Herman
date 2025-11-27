using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
public class GridManager : MonoBehaviour
{
    public const int SIZE = 20;
    public const int LEVELS = 5; // ← ahora 5
    private Vector2Int[] spawns = new Vector2Int[LEVELS];
    private Vector2Int[] exits  = new Vector2Int[LEVELS];

    public Vector2Int GetSpawn(int L) => spawns[Mathf.Clamp(L, 0, LEVELS - 1)];
    public Vector2Int GetExit (int L) => exits[Mathf.Clamp(L, 0, LEVELS - 1)];

    [Header("Tilemaps (asignar en Inspector)")]
    public Tilemap ground;    // Tilemap_Ground
    public Tilemap fog;       // Tilemap_Fog

    [Header("Tiles (asignar en Inspector)")]
    public TileBase tPath;
    public TileBase tWall;
    public TileBase tTrap;
    public TileBase tTreasure;
    public TileBase tExit;
    public TileBase tFog;
    public TileBase tKey;

    [Header("Refs")]
    public PlayerController player;
    public HUDController hud;

    // Mapa lógico: [nivel, x, y]
    public CellType[,,] map = new CellType[LEVELS, SIZE, SIZE];
    public int currentLevel = 0;

    // Registro de celdas que ya tiraron "monedas" (para evitar farmeo)
    private HashSet<string> coinRollDone = new();
    [Header("Densidad del mapa")]
    [Range(0f, 1f)] public float wallDensity = 0.25f;  // porcentaje de muros
    [Range(0f, 1f)] public float trapDensity = 0.10f;
    void Start()
    {
        GenerateAllLevels();

        if (hud == null) hud = FindFirstObjectByType<HUDController>();

        RenderLevel(0);

        // Usa el spawn calculado, no FindFirstPath
        Vector2Int spawn = spawns[0];
        player.Init(this, spawn, 0);
        Reveal(spawn);
    }

    // ======= Generación de laberintos (5 niveles con camino garantizado) =======
    
    void GenerateAllLevels()
{
    coinRollDone.Clear();

    // Distancias mínimas desde el spawn (en casillas) por nivel 0..4
    int[] minKeyDist      = { 4,  6,  8, 10, 12 }; // llave más cerca que el tesoro
    int[] minTreasureDist = { 6,  8, 10, 12, 14 }; // tesoro más lejos que llave
    int[] minExitDist     = { 12, 14, 16, 18, 20 }; // salida bien lejos

    // Densidad de trampas (sube por nivel)
    float[] dens = { 0.06f, 0.09f, 0.12f, 0.16f, 0.20f };

    for (int L = 0; L < LEVELS; L++)
    {
        // 1) Genera mapa + camino solución a una salida (spawn → exit) limpio
        GenerateMazeLevel(L, out Vector2Int spawn, out Vector2Int exit, out List<Vector2Int> solutionPathExit);
        spawns[L] = spawn;

        // Si la salida quedó demasiado cerca, empuja corredor y recalcula
        int minExit = Mathf.Clamp(minExitDist[L], 4, SIZE * 2);
        if (solutionPathExit.Count < minExit)
        {
            Vector2Int target = (L % 2 == 0)
                ? new Vector2Int(SIZE - 2, SIZE - 2)
                : new Vector2Int(SIZE - 2, 1);

            CarveCorridor(L, solutionPathExit[^1], target, 0.25f);

            // Reubica salida en el farthest y recalcula el camino
            exit = FarthestPathFrom(L, spawn, out _);
            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                    if (map[L, x, y] == CellType.Exit) map[L, x, y] = CellType.Path;

            map[L, exit.x, exit.y] = CellType.Exit;
            solutionPathExit = ShortestPathBFS(L, spawn, exit);
        }
        exits[L] = exit;

        // 2) Colocar LLAVE y TESORO sobre el camino solución (siempre limpio)
        int pathLen = solutionPathExit.Count;

        // Indices seguros (evitar extremos)
        int kIdxMin = Mathf.Clamp(minKeyDist[L],  2, Mathf.Max(2, pathLen - 3));
        int tIdxMin = Mathf.Clamp(minTreasureDist[L], kIdxMin + 2, Mathf.Max(kIdxMin + 2, pathLen - 2));

        // Fallback si el camino fuera corto
        if (tIdxMin >= pathLen - 1) tIdxMin = Mathf.Max(2, pathLen - 2);
        if (kIdxMin >= tIdxMin)     kIdxMin = Mathf.Max(2, tIdxMin - 1);

        var keyPos = solutionPathExit[kIdxMin];
        var trePos = solutionPathExit[tIdxMin];

        // Marca KEY (no en spawn/exit por seguridad)
        if (keyPos != spawn && keyPos != exit)
            map[L, keyPos.x, keyPos.y] = CellType.Key;

        // Marca TREASURE (distinto de key/spawn/exit)
        if (trePos != keyPos && trePos != spawn && trePos != exit)
            map[L, trePos.x, trePos.y] = CellType.Treasure;

        // 3) Trampas SOLO fuera del camino solución (para mantener limpio spawn→key→exit)
        SprinkleTraps(L, solutionPathExit, dens[Mathf.Clamp(L, 0, dens.Length - 1)]);
    }
}
    void GenerateMazeLevel(int L, out Vector2Int spawn, out Vector2Int exit, out List<Vector2Int> solutionPath)
    {
        // Genera un nivel orgánico
        GenerateOrganicLevel(L, out spawn, out solutionPath);
        exit = solutionPath[solutionPath.Count - 1];
    }
    

    void GenerateOrganicLevel(int L, out Vector2Int spawn, out List<Vector2Int> solutionPath)
    {
    // --- A) Relleno aleatorio "estilo viejo"
    for (int x = 0; x < SIZE; x++)
        for (int y = 0; y < SIZE; y++)
        {
            float r = Random.value;
if     (r < wallDensity) map[L, x, y] = CellType.Wall;
else if (r < wallDensity + trapDensity) map[L, x, y] = CellType.Trap;
else                                   map[L, x, y] = CellType.Path;
        }

    // --- B) Elige un spawn en PATH (si no hay, fuerzo uno)
    spawn = FindFirstPath(L);
    if (map[L, spawn.x, spawn.y] == CellType.Wall)
        map[L, spawn.x, spawn.y] = CellType.Path;

    // Limpia trampas a 1 de distancia del spawn y garantiza al menos una salida caminable
    SafeSpawn(L, spawn);

    // --- C) Asegura que exista una salida alcanzable
    EnsureExitAndConnectivity(L, spawn, out solutionPath);
}

// Limpia el entorno del spawn y garantiza al menos una celda libre alrededor
void SafeSpawn(int L, Vector2Int spawn)
{
    // Quita trampas en radio 1
    for (int dx = -1; dx <= 1; dx++)
        for (int dy = -1; dy <= 1; dy++)
        {
            int xx = spawn.x + dx, yy = spawn.y + dy;
            if (xx < 0 || yy < 0 || xx >= SIZE || yy >= SIZE) continue;
            if (map[L, xx, yy] == CellType.Trap) map[L, xx, yy] = CellType.Path;
        }

    // Asegura al menos UNA salida libre
    Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    bool hasExit = false;
    foreach (var d in dirs)
    {
        var n = spawn + d;
        if (n.x < 0 || n.y < 0 || n.x >= SIZE || n.y >= SIZE) continue;
        if (map[L, n.x, n.y] != CellType.Wall) { hasExit = true; break; }
    }
    if (!hasExit)
    {
        // abre una puerta al azar
        var d = dirs[Random.Range(0, dirs.Length)];
        var n = spawn + d;
        if (n.x >= 0 && n.y >= 0 && n.x < SIZE && n.y < SIZE)
            map[L, n.x, n.y] = CellType.Path;
    }
}

// Garantiza que exista una salida alcanzable: si no la hay, cava un corredor
void EnsureExitAndConnectivity(int L, Vector2Int spawn, out List<Vector2Int> solutionPath)
{
    // 1) Busca la PATH más lejana desde spawn (sobre celdas NO muro)
    var far = FarthestPathFrom(L, spawn, out _);

    // Si el farthest es el propio spawn, probablemente está aislado: cavar
    if (far == spawn)
    {
        // Elegimos un objetivo lejos (esquina inferior derecha)
        Vector2Int target = new Vector2Int(SIZE - 2, SIZE - 2);
        CarveCorridor(L, spawn, target, wiggle: 0.35f);
        // Recalcula farthest después de cavar
        far = FarthestPathFrom(L, spawn, out _);
    }

    // 2) Marca salida en "far" y calcula camino solución
    // Limpia salida anterior si hubiera
    for (int x = 0; x < SIZE; x++)
        for (int y = 0; y < SIZE; y++)
            if (map[L, x, y] == CellType.Exit) map[L, x, y] = CellType.Path;

    map[L, far.x, far.y] = CellType.Exit;
    solutionPath = ShortestPathBFS(L, spawn, far);

    // Si por algún motivo la ruta no existe (muy raro), cavar directo y recalcular
    if (solutionPath.Count == 0)
    {
        CarveCorridor(L, spawn, far, wiggle: 0.25f);
        map[L, far.x, far.y] = CellType.Exit;
        solutionPath = ShortestPathBFS(L, spawn, far);
    }
}

// Cava un pasillo desde A hacia B convirtiendo muros a PATH con algo de aleatoriedad
void CarveCorridor(int L, Vector2Int a, Vector2Int b, float wiggle = 0.3f)
{
    Vector2Int cur = a;
    int guard = SIZE * SIZE * 4;

    while (cur != b && guard-- > 0)
    {
        // Tendencia hacia el objetivo
        Vector2Int dir = Vector2Int.zero;
        if (Random.value > wiggle)
        {
            // elige el eje con mayor diferencia
            if (Mathf.Abs(b.x - cur.x) > Mathf.Abs(b.y - cur.y))
                dir = (b.x > cur.x) ? Vector2Int.right : Vector2Int.left;
            else
                dir = (b.y > cur.y) ? Vector2Int.up : Vector2Int.down;
        }
        else
        {
            // un pequeño desvío aleatorio
            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            dir = dirs[Random.Range(0, dirs.Length)];
        }

        var nxt = cur + dir;
        if (nxt.x < 1 || nxt.y < 1 || nxt.x >= SIZE - 1 || nxt.y >= SIZE - 1)
        {
            // si se sale, corrige hacia adentro
            nxt = new Vector2Int(Mathf.Clamp(nxt.x, 1, SIZE - 2), Mathf.Clamp(nxt.y, 1, SIZE - 2));
        }

        // tallamos
        if (map[L, nxt.x, nxt.y] == CellType.Wall)
            map[L, nxt.x, nxt.y] = CellType.Path;
        else if (map[L, nxt.x, nxt.y] == CellType.Trap)
            map[L, nxt.x, nxt.y] = CellType.Path; // no quiero corredor con trampa

        cur = nxt;
    }
    }

    public Vector2Int FindFirstPath(int L)
    {
        for (int x = 0; x < SIZE; x++)
            for (int y = 0; y < SIZE; y++)
                if (map[L, x, y] == CellType.Path) return new Vector2Int(x, y);
        return Vector2Int.zero;
    }

    // ======= Render en Tilemaps =======
    public void RenderLevel(int L)
    {
        ground.ClearAllTiles();
        fog.ClearAllTiles();

        for (int x = 0; x < SIZE; x++)
        for (int y = 0; y < SIZE; y++)
        {
            TileBase tb = tPath;
            switch (map[L, x, y])
            {
                case CellType.Path:     tb = tPath; break;
                case CellType.Wall:     tb = tWall; break;
                case CellType.Trap:     tb = tTrap; break;
                case CellType.Treasure: tb = tTreasure; break;
                case CellType.Exit:     tb = tExit; break;
                case CellType.Key:      tb = tKey; break;    
            }

            var p = new Vector3Int(x, y, 0);
            ground.SetTile(p, tb);
            fog.SetTile(p, tFog); // niebla cubre todo
        }
    }

    public void Reveal(Vector2Int gp)
    {
        fog.SetTile(new Vector3Int(gp.x, gp.y, 0), null); // quita la niebla para siempre
    }

    public bool IsWalkable(int L, Vector2Int gp)
    {
        if (gp.x < 0 || gp.y < 0 || gp.x >= SIZE || gp.y >= SIZE) return false;
        return map[L, gp.x, gp.y] != CellType.Wall;
    }

    // ======= Interacciones =======
    public void OnStepped(int L, Vector2Int gp)
    {
        var c = map[L, gp.x, gp.y];

        if (c == CellType.Trap)
        {
            GameManager.I.OnTrap();
            map[L, gp.x, gp.y] = CellType.Path;
            ground.SetTile((Vector3Int)gp, tPath);
        }
        else if (c == CellType.Treasure)
        {
            // Intenta guardarlo como objeto (ocupa slot)
            bool picked = false;
            if (player != null && player.inv != null)
                picked = player.inv.Add(ItemType.Treasure, 1);

            if (picked)
            {
                GameManager.I.SetKeyCollected(L, true);
                GameManager.I.CollectTreasure();     // sube el contador (para multiplicador)
                map[L, gp.x, gp.y] = CellType.Path;  // ya no hay tesoro
                ground.SetTile((Vector3Int)gp, tPath);
                if (hud) hud.FlashGold(1.5f);
            }
            else
            {
                // Inventario lleno: no lo recojas, deja el tile
                GameManager.I.banner?.Show("Inventario lleno", 1.0f);
            }
        }
else if (c == CellType.Key)
{
    GameManager.I.SetKeyCollected(L, true);
    map[L, gp.x, gp.y] = CellType.Path;
    ground.SetTile((Vector3Int)gp, tPath);

    GameManager.I.banner?.Show("¡Has conseguido la llave!", 2f);
    if (hud) hud.FlashGold(1.2f);
}
        else if (c == CellType.Exit)
{
    if (!GameManager.I.IsKeyCollected(L))
        GameManager.I.banner?.Show("La puerta está cerrada. Necesitas la llave.", 1.4f);
    else
        GameManager.I.NextLevelOrWin();
}
        else if (c == CellType.Treasure)
        {
            bool picked = false;
            if (player != null && player.inv != null)
                picked = player.inv.Add(ItemType.Treasure, 1);

            if (picked)
            {
                GameManager.I.CollectTreasure();
                map[L, gp.x, gp.y] = CellType.Path;
                ground.SetTile((Vector3Int)gp, tPath);
                hud?.FlashGold(1.5f);
            }

            else
            {
                GameManager.I.banner?.Show("Inventario lleno", 1.0f);
            }
        }

        // Solo intenta dar monedas la PRIMERA vez que se pisa
        TryAwardPastiOnce(L, gp);
    }

    // ======= Monedas (Pastis) =======
    private void TryAwardPastiOnce(int L, Vector2Int p)
{
    string key = $"{L}:{p.x},{p.y}";
    if (coinRollDone.Contains(key)) return;

    coinRollDone.Add(key);
    if (Random.value < 0.15f)
    {
        int gain = Random.Range(3, 21);
        GameManager.I.AddPasti(gain); // ✅ solo Pastis
        // ❌ nada de AddScore aquí
    }
}

    // ======= Export/Import para guardado =======
    public string ExportMap()
    {
        System.Text.StringBuilder sb = new();
        for (int L = 0; L < LEVELS; L++)
            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                {
                    int v = map[L, x, y] switch
                    {
                        CellType.Path => 0,
                        CellType.Wall => 1,
                        CellType.Trap => 2,
                        CellType.Treasure => 3,
                        CellType.Exit => 4,
                        CellType.Key => 5,
                        _ => 0
                    };
                    sb.Append((char)('0' + v));
                }
        return sb.ToString();
    }

    public void ImportMap(string data)
    {
        if (string.IsNullOrEmpty(data)) return;
        int i = 0;
        for (int L = 0; L < LEVELS; L++)
            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                {
                    int v = data[i++] - '0';
                    map[L, x, y] = v switch
                    {
                        0 => CellType.Path,
                        1 => CellType.Wall,
                        2 => CellType.Trap,
                        3 => CellType.Treasure,
                        4 => CellType.Exit,
                        5 => CellType.Key,
                        _ => CellType.Path
                    };
                }
        RenderLevel(currentLevel);
    }

    public void RevealAhead(Vector2Int from, Vector2Int dir, int dist)
    {
        if (dir == Vector2Int.zero) return;
        for (int i = 1; i <= dist; i++)
        {
            var p = from + dir * i;
            if (p.x < 0 || p.y < 0 || p.x >= SIZE || p.y >= SIZE) break;
            Reveal(p);
            if (map[currentLevel, p.x, p.y] == CellType.Wall) break; // la pared bloquea la luz
        }
    }

    // ======= Generación de laberinto (DFS) + camino garantizado =======
    

    Vector2Int FarthestPathFrom(int L, Vector2Int start, out Dictionary<Vector2Int, Vector2Int> parent)
    {
        Queue<Vector2Int> q = new();
        parent = new();
        HashSet<Vector2Int> vis = new();

        q.Enqueue(start);
        vis.Add(start);
        Vector2Int last = start;

        Vector2Int[] dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (q.Count > 0)
        {
            var u = q.Dequeue();
            last = u;
            foreach (var d in dirs)
            {
                var v = u + d;
                if (v.x < 0 || v.y < 0 || v.x >= SIZE || v.y >= SIZE) continue;
                if (vis.Contains(v)) continue;
                if (map[L, v.x, v.y] == CellType.Wall) continue; // solo caminos

                vis.Add(v);
                parent[v] = u;
                q.Enqueue(v);
            }
        }
        return last;
    }

    List<Vector2Int> ShortestPathBFS(int L, Vector2Int a, Vector2Int b)
    {
        Queue<Vector2Int> q = new();
        Dictionary<Vector2Int, Vector2Int> parent = new();
        HashSet<Vector2Int> vis = new();

        q.Enqueue(a);
        vis.Add(a);

        Vector2Int[] dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        bool found = false;

        while (q.Count > 0)
        {
            var u = q.Dequeue();
            if (u == b) { found = true; break; }
            foreach (var d in dirs)
            {
                var v = u + d;
                if (v.x < 0 || v.y < 0 || v.x >= SIZE || v.y >= SIZE) continue;
                if (vis.Contains(v)) continue;
                if (map[L, v.x, v.y] == CellType.Wall) continue;

                vis.Add(v);
                parent[v] = u;
                q.Enqueue(v);
            }
        }

        List<Vector2Int> path = new();
        if (!found) return path;
        var cur = b;
        while (cur != a)
        {
            path.Add(cur);
            cur = parent[cur];
        }
        path.Add(a);
        path.Reverse();
        return path;
    }

    void SprinkleTraps(int L, List<Vector2Int> solution, float density)
    {
        HashSet<Vector2Int> sol = new(solution);
        for (int x = 0; x < SIZE; x++)
            for (int y = 0; y < SIZE; y++)
            {
                if (map[L, x, y] != CellType.Path) continue;
                var p = new Vector2Int(x, y);
                if (sol.Contains(p)) continue; // ❗ no ensuciar el camino spawn→exit (tesoro queda sobre él)
                if (Random.value < density)
                    map[L, x, y] = CellType.Trap;
            }
    }


}
