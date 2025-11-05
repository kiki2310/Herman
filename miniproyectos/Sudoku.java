import java.util.*;

public class Sudoku {

    public enum NivelDificultad {
        MUY_FACIL(36, 44),
        FACIL(32, 35),
        NORMAL(28, 31),
        DIFICIL(24, 27),
        MUY_DIFICIL(17, 23);

        private final int minPistas;
        private final int maxPistas;

        NivelDificultad(int min, int max) {
            this.minPistas = min;
            this.maxPistas = max;
        }

        public int getMinPistas() { return minPistas; }
        public int getMaxPistas() { return maxPistas; }
    }

    private static final int TAMANO = 9;
    private static final int NUM_NIVELES = 5;

    // Solución base (la usaremos solo como "semilla")
    private static final int[][] SOLUCION_BASE = {
        {5, 3, 4, 6, 7, 8, 9, 1, 2},
        {6, 7, 2, 1, 9, 5, 3, 4, 8},
        {1, 9, 8, 3, 4, 2, 5, 6, 7},
        {8, 5, 9, 7, 6, 1, 4, 2, 3},
        {4, 2, 6, 8, 5, 3, 7, 9, 1},
        {7, 1, 3, 9, 2, 4, 8, 5, 6},
        {9, 6, 1, 5, 3, 7, 2, 8, 4},
        {2, 8, 7, 4, 1, 9, 6, 3, 5},
        {3, 4, 5, 2, 8, 6, 1, 7, 9}
    };

    // puzzles[nivel] = tablero con huecos
    private final int[][][] puzzles;
    // soluciones[nivel] = tablero completo correcto
    private final int[][][] soluciones;
    // celdas fijas
    private final boolean[][][] celdasFijas;

    private int vidas;
    int nivelActual;
    private int puntaje;

    public Sudoku() {
        this.puzzles = new int[NUM_NIVELES][TAMANO][TAMANO];
        this.soluciones = new int[NUM_NIVELES][TAMANO][TAMANO];
        this.celdasFijas = new boolean[NUM_NIVELES][TAMANO][TAMANO];
        this.vidas = 5;
        this.nivelActual = 0;
        this.puntaje = 0;
    }

    public void generarPuzzles() {
        NivelDificultad[] niveles = NivelDificultad.values();
        Random rand = new Random();

        for (int i = 0; i < NUM_NIVELES; i++) {
            NivelDificultad nivel = niveles[i];
            int pistas = rand.nextInt(nivel.getMaxPistas() - nivel.getMinPistas() + 1) + nivel.getMinPistas();

            // 1. generar una solución aleatoria basada en la solución base
            int[][] solucionAleatoria = generarSolucionAleatoria(rand);

            // 2. guardarla
            for (int f = 0; f < TAMANO; f++) {
                soluciones[i][f] = Arrays.copyOf(solucionAleatoria[f], TAMANO);
            }

            // 3. crear el puzzle (quitar números) a partir de ESA solución
            puzzles[i] = crearPuzzleDesdeSolucion(solucionAleatoria, pistas, i);
        }
    }


    private int[][] generarSolucionAleatoria(Random rand) {
        // empezamos copiando la base
        int[][] s = new int[TAMANO][TAMANO];
        for (int i = 0; i < TAMANO; i++) {
            s[i] = Arrays.copyOf(SOLUCION_BASE[i], TAMANO);
        }

        // 1. permutar dígitos 1..9
        int[] permDigitos = generarPermutacionDigitos(rand);
        for (int i = 0; i < TAMANO; i++) {
            for (int j = 0; j < TAMANO; j++) {
                int val = s[i][j];
                s[i][j] = permDigitos[val]; // val está entre 1 y 9
            }
        }

        for (int banda = 0; banda < 3; banda++) {
            permutarFilasEnBanda(s, banda * 3, rand);
        }

        for (int stack = 0; stack < 3; stack++) {
            permutarColumnasEnStack(s, stack * 3, rand);
        }

        permutarBandasFilas(s, rand);

        permutarStacksColumnas(s, rand);

        return s;
    }

    private int[] generarPermutacionDigitos(Random rand) {
        int[] p = new int[10];
        List<Integer> nums = new ArrayList<>();
        for (int i = 1; i <= 9; i++) nums.add(i);
        Collections.shuffle(nums, rand);
        for (int i = 1; i <= 9; i++) {
            p[i] = nums.get(i - 1);
        }
        return p;
    }

    private void permutarFilasEnBanda(int[][] s, int filaInicio, Random rand) {
        List<Integer> filas = Arrays.asList(filaInicio, filaInicio + 1, filaInicio + 2);
        Collections.shuffle(filas, rand);
        int[][] copia = new int[3][TAMANO];
        for (int i = 0; i < 3; i++) copia[i] = Arrays.copyOf(s[filaInicio + i], TAMANO);
        for (int i = 0; i < 3; i++) s[filaInicio + i] = Arrays.copyOf(copia[filas.get(i) - filaInicio], TAMANO);
    }

    private void permutarColumnasEnStack(int[][] s, int colInicio, Random rand) {
        List<Integer> cols = Arrays.asList(colInicio, colInicio + 1, colInicio + 2);
        Collections.shuffle(cols, rand);
        for (int fila = 0; fila < TAMANO; fila++) {
            int[] copia = Arrays.copyOf(s[fila], TAMANO);
            for (int i = 0; i < 3; i++) {
                s[fila][colInicio + i] = copia[cols.get(i)];
            }
        }
    }

    private void permutarBandasFilas(int[][] s, Random rand) {
        List<Integer> bandas = Arrays.asList(0, 1, 2);
        Collections.shuffle(bandas, rand);
        int[][] copia = new int[TAMANO][TAMANO];
        for (int i = 0; i < TAMANO; i++) copia[i] = Arrays.copyOf(s[i], TAMANO);
        for (int b = 0; b < 3; b++) {
            int origen = bandas.get(b) * 3;
            for (int k = 0; k < 3; k++) {
                s[b * 3 + k] = Arrays.copyOf(copia[origen + k], TAMANO);
            }
        }
    }

    private void permutarStacksColumnas(int[][] s, Random rand) {
        List<Integer> stacks = Arrays.asList(0, 1, 2);
        Collections.shuffle(stacks, rand);
        for (int fila = 0; fila < TAMANO; fila++) {
            int[] copia = Arrays.copyOf(s[fila], TAMANO);
            for (int st = 0; st < 3; st++) {
                int origen = stacks.get(st) * 3;
                s[fila][st * 3]     = copia[origen];
                s[fila][st * 3 + 1] = copia[origen + 1];
                s[fila][st * 3 + 2] = copia[origen + 2];
            }
        }
    }

    private int[][] crearPuzzleDesdeSolucion(int[][] solucion, int pistas, int indiceNivel) {
        int[][] puzzle = new int[TAMANO][TAMANO];
        for (int i = 0; i < TAMANO; i++) {
            puzzle[i] = Arrays.copyOf(solucion[i], TAMANO);
        }

        for (int i = 0; i < TAMANO; i++) {
            for (int j = 0; j < TAMANO; j++) {
                celdasFijas[indiceNivel][i][j] = false;
            }
        }

        Random rand = new Random();
        int celdasAEliminar = TAMANO * TAMANO - pistas;

        for (int k = 0; k < celdasAEliminar; k++) {
            int fila, col;
            do {
                fila = rand.nextInt(TAMANO);
                col = rand.nextInt(TAMANO);
            } while (puzzle[fila][col] == 0);
            puzzle[fila][col] = 0;
        }

        // marcar fijas
        for (int i = 0; i < TAMANO; i++) {
            for (int j = 0; j < TAMANO; j++) {
                if (puzzle[i][j] != 0) {
                    celdasFijas[indiceNivel][i][j] = true;
                }
            }
        }

        return puzzle;
    }

    public void imprimirTodasLasSoluciones() {
        System.out.println("=== SOLUCIONES GENERADAS ===");
        for (int n = 0; n < NUM_NIVELES; n++) {
            System.out.println("\nSOLUCIÓN NIVEL " + (n + 1) + ":");
            imprimirSolucion(n);
        }
        System.out.println("=== FIN ===");
    }

    private void imprimirSolucion(int nivel) {
        int[][] sol = soluciones[nivel];
        for (int i = 0; i < TAMANO; i++) {
            if (i % 3 == 0 && i != 0) System.out.println("-----------------------");
            for (int j = 0; j < TAMANO; j++) {
                if (j % 3 == 0 && j != 0) System.out.print(" | ");
                System.out.print(sol[i][j] + " ");
            }
            System.out.println();
        }
    }

    public void imprimirTablero(int nivel) {
        int[][] tablero = puzzles[nivel];
        for (int i = 0; i < TAMANO; i++) {
            if (i % 3 == 0 && i != 0) {
                System.out.println("-----------------------");
            }
            for (int j = 0; j < TAMANO; j++) {
                if (j % 3 == 0 && j != 0) {
                    System.out.print(" | ");
                }
                if (celdasFijas[nivel][i][j]) {
                    System.out.print("\u001B[34m" + tablero[i][j] + "\u001B[0m ");
                } else {
                    System.out.print((tablero[i][j] == 0 ? "." : tablero[i][j]) + " ");
                }
            }
            System.out.println();
        }
    }


    private boolean esNumeroCorrectoSegunSolucion(int nivel, int fila, int col, int num) {
        return soluciones[nivel][fila][col] == num;
    }

    private boolean nivelCompletado(int nivel) {
        for (int i = 0; i < TAMANO; i++) {
            for (int j = 0; j < TAMANO; j++) {
                if (puzzles[nivel][i][j] == 0) {
                    return false;
                }
            }
        }
        return true;
    }

   public void jugarNivel() {
    Scanner scanner = new Scanner(System.in);
    NivelDificultad nivelInfo = NivelDificultad.values()[nivelActual];

    while (true) {
        System.out.println("\n--- NIVEL " + (nivelActual + 1) + ": " + nivelInfo + " ---");
        System.out.println("--- Vidas: " + vidas + " | Puntaje: " + puntaje + " ---");
        imprimirTablero(nivelActual);

        System.out.print("fila col num (1-9): ");
        try {
            int fila = scanner.nextInt() - 1;
            int col  = scanner.nextInt() - 1;
            int num  = scanner.nextInt();

            // Rango estricto 1..9
            if (fila < 0 || fila >= TAMANO || col < 0 || col >= TAMANO || num < 1 || num > 9) {
                System.out.println("Fuera de rango. Fila/col 1-9 y número 1-9.");
                continue;
            }

            if (celdasFijas[nivelActual][fila][col]) {
                System.out.println("Esa celda es fija; no se puede modificar.");
                continue;
            }

            int valorActual = puzzles[nivelActual][fila][col];

            if (valorActual != 0) {
                if (valorActual == num) {
                    System.out.println("Ya estaba ese número en la celda.");
                } else {
                    System.out.println("La celda ya está ocupada; no puedes sobrescribirla.");
                }
                continue;
            }

            if (esNumeroCorrectoSegunSolucion(nivelActual, fila, col, num)) {
                puzzles[nivelActual][fila][col] = num;
                int puntosGanados = (nivelActual + 1) * 10;
                puntaje += puntosGanados;
                System.out.println("¡Correcto! +" + puntosGanados + " puntos.");
            } else {
                vidas--;
                System.out.println("No es el número correcto. -1 vida.");
                if (vidas <= 0) {
                    System.out.println("GAME OVER");
                    return;
                }
            }

            if (nivelCompletado(nivelActual)) {
                imprimirTablero(nivelActual);
                puntaje += 500;
                System.out.println("¡Bono de 500 puntos por completar el nivel!");
                System.out.println("¡Felicidades! Nivel completado. Puntaje actual: " + puntaje);
                System.out.println("Nivel completado.");
                nivelActual++;
                vidas = 5;
                if (nivelActual >= NUM_NIVELES) {
                    System.out.println("¡Todos los niveles listos!");
                }
                return;
            }

        } catch (InputMismatchException e) {
            System.out.println("Entrada inválida.");
            scanner.next();
        }
    }
}

    public static void main(String[] args) {
        Sudoku juego = new Sudoku();
        System.out.println("¡Bienvenido al Sudoku!");
        juego.generarPuzzles();

        juego.imprimirTodasLasSoluciones();

        while (juego.nivelActual < NUM_NIVELES && juego.vidas > 0) {
            juego.jugarNivel();
        }

        System.out.println("Gracias por jugar. Tu puntaje final es: " + juego.puntaje);
        
    }
}
