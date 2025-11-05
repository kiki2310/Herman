using System;

public class ShellSort {
    // Una función de utilidad para imprimir un arreglo
    public static void DisplayArr(int[] inputArr) {
        Console.WriteLine(string.Join(" ", inputArr));
    }

    // Función para ordenar inputArr usando Shell Sort
    public void Sort(int[] inputArr) {
        int size = inputArr.Length;
        // Comienza con un espacio grande y luego reduce el espacio
        for (int gapSize = size / 2; gapSize > 0; gapSize /= 2) {
            
            // Realiza un ordenamiento por inserción con espacios
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
    public static void Main(string[] args) {
        int[] inputArr = {36, 34, 43, 11, 15, 20, 28, 45};
        Console.WriteLine("Arreglo antes de ser ordenado:");
        DisplayArr(inputArr);

        ShellSort obj = new ShellSort();
        obj.Sort(inputArr);

        Console.WriteLine("Arreglo después de ser ordenado:");
        DisplayArr(inputArr);
    }
}
