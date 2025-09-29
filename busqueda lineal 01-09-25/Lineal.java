import java.util.Scanner;

public class Lineal {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        int[] arreglo = {10, 20, 30, 40, 50};
        imprimirArreglo(arreglo);
    
            System.out.print("Introduce el elemento a buscar: ");
            int objetivo = scanner.nextInt();
            
            int indice = busquedaLineal(arreglo, objetivo);

            if (indice != -1) {
                System.out.println("Elemento encontrado en el índice: " + indice);
            } else {
                System.out.println("Elemento no encontrado.");
            }

            System.out.println("Valor para insertar:");
            int nuevoValor = scanner.nextInt();
            System.out.println("Posición para insertar:");
            int posicion = scanner.nextInt();


            insertarnumero(arreglo, nuevoValor, posicion);
            imprimirArreglo(arreglo);
            scanner.close();
            System.out.println("fin del programa");

    }

    public static int busquedaLineal(int[] arreglo, int objetivo) {
        for (int i = 0; i < arreglo.length; i++) {
            if (arreglo[i] == objetivo) {
                return i; // Retorna el índice si se encuentra el elemento
            }
        }
        return -1; // Retorna -1 si el elemento no se encuentra
    }

    static void imprimirArreglo(int[] arreglo) {
        System.out.print("Arreglo: ");
        for (int num : arreglo) {
            System.out.print(num + " ");
        }
        System.out.println();
    }
    static void insertarnumero(int[] arreglo, int numero, int posicion) {
        if (posicion < 0 || posicion >= arreglo.length) {
            System.out.println("Posición inválida.");
            return;
        }
        arreglo[posicion] = numero;
    }
}
