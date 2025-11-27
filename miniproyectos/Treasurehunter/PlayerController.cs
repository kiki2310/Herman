using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public bool AwaitBallTarget = false;  // ← NUEVO
    public Vector2Int GridPos { get; private set; }
    public int Level { get; private set; }
    public float moveSpeed = 14f;
    
    private GridManager grid;
    private GameManager gm;
    private Vector3 targetWorld;

    private bool moving = false;

    public HashSet<Vector2Int> VisitedCells { get; private set; } = new();

    [Header("Power-ups")]
    public bool pendingDash = false;               // activado si usas bebida energética
    public Vector2Int lastDir = Vector2Int.zero;   // última dirección pulsada
    public Inventory inv;                           // arrastra el Inventory del Player

    public void Init(GridManager g, Vector2Int start, int level)
    {
        grid = g; Level = level; GridPos = start;
        targetWorld = ToWorld(GridPos);
        transform.position = targetWorld;
        moving = false;

        VisitedCells.Clear();
        MarkVisited(GridPos);
        grid.Reveal(GridPos);
    }

    void Awake() { gm = GameManager.I; }
    void Start() { if (gm == null) gm = GameManager.I; }

    void Update()
    {
        // Modo apuntar con Pelota
        // Modo apuntar (Pelota)
if (AwaitBallTarget)
{
    if (Input.GetMouseButtonDown(0))
    {
        var cam = Camera.main;
        if (cam != null)
        {
            Vector3 wp = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = grid.ground.WorldToCell(wp);
            Vector2Int gp = new Vector2Int(cell.x, cell.y);

            if (gp.x >= 0 && gp.y >= 0 && gp.x < GridManager.SIZE && gp.y < GridManager.SIZE)
            {
                grid.Reveal(gp);                     // revela para siempre
                // opcional: grid.RevealNeighbors(gp, 1);
                GameManager.I?.banner?.Show("Casilla revelada", 0.8f);

                // gastar el “turno” del uso
                GameManager.I?.OnTurnEnded();
                inv?.OnTurnEnded();
            }
        }
        AwaitBallTarget = false; // sal del modo apuntar
    }
    return; // mientras apuntas, no mueves
    }

        if (gm == null) { gm = GameManager.I; if (gm == null) return; }
        if (!gm.Running) return;

        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorld, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetWorld) < 0.001f) moving = false;
            return;
        }

        Vector2Int dir = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) dir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) dir = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) dir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2Int.right;

        if (dir != Vector2Int.zero) TryMove(dir);
    }

    void TryMove(Vector2Int dir)
    {
        if (grid == null) return;

        lastDir = dir; // guarda hacia dónde se intentó mover

        // Paso 1: si hay dash, intenta mover 2 celdas; si no, 1.
        Vector2Int step1 = GridPos + dir;
        Vector2Int step2 = GridPos + dir + dir;
        Vector2Int dest = GridPos;

        if (pendingDash)
        {
            if (grid.IsWalkable(Level, step1) && grid.IsWalkable(Level, step2))
                dest = step2;        // 2 celdas
            else if (grid.IsWalkable(Level, step1))
                dest = step1;        // solo 1 si la segunda está bloqueada
            else
            {
                // choca con pared en el primer paso
                gm.OnWallBump();
                gm.OnTurnEnded();
                if (inv != null) inv.OnTurnEnded();
                pendingDash = false;
                return;
            }
            pendingDash = false;     // consumir el dash
        }
        else
        {
            if (!grid.IsWalkable(Level, step1))
            {
                grid.Reveal(step1);  // ← quita la niebla en esa casilla y ya no vuelve

                gm.OnWallBump();
                gm.OnTurnEnded();
                if (inv) inv.OnTurnEnded();
                return;
            }
            dest = step1;
        }

        // Mover
        GridPos = dest;
        targetWorld = ToWorld(GridPos);
        moving = true;

        // Revelar casilla actual y marcar visitada
        grid.Reveal(GridPos);
        MarkVisited(GridPos);

        // Linterna: revela 2 casillas al frente si está activa
        if (inv != null && inv.flashlightTurns > 0)
        grid.RevealAhead(GridPos, lastDir, 2);

        // Eventos de celda
        grid.OnStepped(Level, GridPos);

        // Fin de turno: reglas de energía + baja turnos de linterna
        gm.OnTurnEnded();
        if (inv != null) inv.OnTurnEnded();
    }

    void MarkVisited(Vector2Int p) => VisitedCells.Add(p);
    Vector3 ToWorld(Vector2Int gp) => new Vector3(gp.x + 0.5f, gp.y + 0.5f, 0f);
}
