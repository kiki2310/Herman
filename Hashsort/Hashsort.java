public class Hashsort {

    // Una función de utilidad para imprimir un arreglo
    static void displayArr(int[] inputArr) {
        for (int k : inputArr) {
            System.out.print(k + " ");
        }
        System.out.println();
    }

    // Función para ordenar inputArr usando Shell Sort
    public void sort(int[] inputArr) {
        int size = inputArr.length;
        // Comienza con un espacio grande y luego reduce el espacio
        for (int gapSize = size / 2; gapSize > 0; gapSize /= 2) {
            // Realiza un ordenamiento por inserción con espacios para este tamaño de espacio.
            // Los primeros elementos de espacio de la matriz inputArr[0..gap-1] ya están en orden.
            // Ahora, seguimos añadiendo un elemento más hasta que toda la matriz esté ordenada.
            for (int j = gapSize; j < size; j++) {
                int val = inputArr[j];
                int k = j;

                // Compara y mueve elementos mientras el anterior (a una distancia 'gapSize') sea mayor
                while (k >= gapSize && inputArr[k - gapSize] > val) {
                    inputArr[k] = inputArr[k - gapSize];
                    k -= gapSize;
                }

                // Inserta el elemento en su posición correcta
                inputArr[k] = val;
            }
        }
    }

    // Bloque principal
    public static void main(String[] args) {
        int[] inputArr = {36, 34, 43, 11, 15, 20, 28, 45};
        System.out.println("Arreglo antes de ser ordenado:");
        displayArr(inputArr);

        Hashsort obj = new Hashsort();
        obj.sort(inputArr);

        System.out.println("Arreglo después de ser ordenado:");
        displayArr(inputArr);
    }
}
