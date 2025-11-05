using System;

public class MergeSort {

    // Combina dos subarreglos de arr[].
    void Merge(int[] arr, int l, int m, int r) {
        int n1 = m - l + 1;
        int n2 = r - m;

        // Crea arreglos temporales
        int[] L = new int[n1];
        int[] R = new int[n2];

        // Copia los datos a los arreglos temporales
        for (int i = 0; i < n1; ++i)
            L[i] = arr[l + i];
        for (int j = 0; j < n2; ++j)
            R[j] = arr[m + 1 + j];

        int i_L = 0, j_R = 0;
        int k = l;

        // Combina los arreglos temporales
        while (i_L < n1 && j_R < n2) {
            if (L[i_L] <= R[j_R]) {
                arr[k] = L[i_L];
                i_L++;
            } else {
                arr[k] = R[j_R];
                j_R++;
            }
            k++;
        }

        // Copia los elementos restantes de L[]
        while (i_L < n1) {
            arr[k] = L[i_L];
            i_L++;
            k++;
        }

        // Copia los elementos restantes de R[]
        while (j_R < n2) {
            arr[k] = R[j_R];
            j_R++;
            k++;
        }
    }

    // Función principal que ordena arr[l..r]
    void Sort(int[] arr, int l, int r) {
        if (l < r) {
            int m = l + (r - l) / 2;

            Sort(arr, l, m);
            Sort(arr, m + 1, r);

            Merge(arr, l, m, r);
        }
    }

    // Método de utilidad para imprimir el arreglo
    static void PrintArray(int[] arr) {
        Console.WriteLine(string.Join(" ", arr));
    }

    // Código de ejecución
    public static void Main(string[] args) {
        int[] arr = { 39, 28, 44, 11 };

        Console.WriteLine("Arreglo antes de ordenar:");
        PrintArray(arr);

        MergeSort ob = new MergeSort();
        ob.Sort(arr, 0, arr.Length - 1);

        Console.WriteLine("\nArreglo después de ordenar:");
        PrintArray(arr);
    }
}
