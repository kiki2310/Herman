public class MergeSort {

    // Combina dos subarreglos de arr[].
    // El primer subarreglo es arr[l..m]
    // El segundo subarreglo es arr[m+1..r]
    void merge(int arr[], int l, int m, int r) {
        // Encuentra los tamaños de los dos subarreglos a combinar
        int n1 = m - l + 1;
        int n2 = r - m;

        // Crea arreglos temporales
        int L[] = new int[n1];
        int R[] = new int[n2];

        // Copia los datos a los arreglos temporales
        for (int i = 0; i < n1; ++i)
            L[i] = arr[l + i];
        for (int j = 0; j < n2; ++j)
            R[j] = arr[m + 1 + j];

        // Combina los arreglos temporales

        // Índices iniciales del primer y segundo subarreglo
        int i = 0, j = 0;

        // Índice inicial del subarreglo combinado
        int k = l;
        while (i < n1 && j < n2) {
            if (L[i] <= R[j]) {
                arr[k] = L[i];
                i++;
            } else {
                arr[k] = R[j];
                j++;
            }
            k++;
        }

        // Copia los elementos restantes de L[], si hay alguno
        while (i < n1) {
            arr[k] = L[i];
            i++;
            k++;
        }

        // Copia los elementos restantes de R[], si hay alguno
        while (j < n2) {
            arr[k] = R[j];
            j++;
            k++;
        }
    }

    // Función principal que ordena arr[l..r] usando merge()
    void sort(int arr[], int l, int r) {
        if (l < r) {
            // Encuentra el punto medio
            int m = l + (r - l) / 2;

            // Ordena la primera y segunda mitad
            sort(arr, l, m);
            sort(arr, m + 1, r);

            // Combina las mitades ordenadas
            merge(arr, l, m, r);
        }
    }

    // Método de utilidad para imprimir el arreglo
    static void printArray(int arr[]) {
        for (int i = 0; i < arr.length; ++i)
            System.out.print(arr[i] + " ");
        System.out.println();
    }

    // Código de ejecución
    public static void main(String args[]) {
        int arr[] = { 39, 28, 44, 11 };

        System.out.println("Arreglo antes de ordenar:");
        printArray(arr);

        MergeSort ob = new MergeSort();
        ob.sort(arr, 0, arr.length - 1);

        System.out.println("\nArreglo después de ordenar:");
        printArray(arr);
    }
}
