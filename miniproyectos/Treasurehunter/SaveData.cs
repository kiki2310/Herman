using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // Estado del juego
    public int hp, energy, maxEnergy;
    public int levelIndex;
    public int treasures;
    public int pasti;
    public int score;
    public float timeLeft;

    // Player
    public int px, py;    // posición en grid
    public int plevel;    // nivel del jugador (0..2)

    // Mapa completo (3*20*20) aplanado como string de dígitos: 0..4
    // 0=Path, 1=Wall, 2=Trap, 3=Treasure, 4=Exit
    public string map;

    // Celdas visitadas (para niebla del nivel actual)
    public List<string> visited = new();
     public string keys;
}
