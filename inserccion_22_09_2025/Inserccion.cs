using System;

public class Inserccion   
{
    public static void Main(string[] args)
    {
        int[] arr = {64, 34, 25, 12, 22, 11, 90};
        OrdenarPorInsercion(arr);
        Console.WriteLine("arreglo ordenado: " + string.Join(", ", arr));
    }
    public static void OrdenarPorInsercion(int[] arr)
    {
        int n = arr.Length;
        for (int i = 1; i < n; i++)
        {
            int key = arr[i];
            int j = i - 1;
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j = j - 1;
            }
            arr[j + 1] = key;
        }
    }
}