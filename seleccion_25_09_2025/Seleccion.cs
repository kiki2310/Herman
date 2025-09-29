using System;

public class Seleccion
{
    public static void Main(string[] args)
    {
        int[] arr = { 64, 25, 12, 22, 11 };
        OrdenarPorSeleccion(arr);
        Console.WriteLine("arreglo ordenado: " + string.Join(", ", arr));
    }
    public static void OrdenarPorSeleccion(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int min_idx = i;
            for (int j = i + 1; j < n; j++)
                if (arr[j] < arr[min_idx])
                    min_idx = j;
            (arr[i], arr[min_idx]) = (arr[min_idx], arr[i]);
        }
    }
}