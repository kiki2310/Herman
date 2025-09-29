public class matriz { 
    public static void main(String[] args) {
        int[][] matriz = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        
        int filas = matriz.length;
        int columnas = matriz[0].length;
        
        System.out.println("--- Matriz Original ---");
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                System.out.print(matriz[i][j] + " ");
            }
            System.out.println();
        }

        System.out.println("\n--- columnas ---");
        for (int i = 0; i < columnas; i++) {
            for (int j = 0; j < filas; j++) {
                System.out.print(matriz[j][i] + " ");
            }
        }
        System.out.println("\n--- filas ---");
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                System.out.print(matriz[i][j] + " ");
            }
        }
            System.out.println(); 


        System.out.println("\nfin del programa");
    }
}
