using System;

public class Burbuja   
{
    public static void Main(string[] args)
    {
        int[] arr = {64, 34, 25, 12, 22, 11, 90};
        Burbujafuncion(arr);
        Console.WriteLine("arreglo ordenado: " + string.Join(", ", arr));
    }
    public static void Burbujafuncion(int[] arr)
    {
        int n = arr.Length;
        int i, j, vaso;
        for (i = 0; i < n - 1; i++)
        {
            for (j = 0; j < n - i - 1; j++) 
            {
                if (arr[j] > arr[j + 1])
                {
                    vaso = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = vaso;
                }
            }
        }
    }
}

