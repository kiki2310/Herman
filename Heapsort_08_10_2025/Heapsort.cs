using System;

public class Heapsort
{
    public static void Heapify(int[] arr, int n, int i)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < n && arr[left] > arr[largest])
        {
            largest = left;
        }

        if (right < n && arr[right] > arr[largest])
        {
            largest = right;
        }

        if (largest != i)
        {
            (arr[i], arr[largest]) = (arr[largest], arr[i]);
            Heapify(arr, n, largest);
        }
    }

    public static void Sort(int[] arr)
    {
        int n = arr.Length;

        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(arr, n, i);
        }

        for (int i = n - 1; i > 0; i--)
        {
            (arr[0], arr[i]) = (arr[i], arr[0]);
            Heapify(arr, i, 0);
        }
    }

    public static void Main(string[] args)
    {
        int[] lista = { 12, 11, 13, 5, 6, 7 };
        Console.WriteLine("Antes de ordenar los elementos del array son: [" + string.Join(", ", lista) + "]");
        Sort(lista);
        Console.WriteLine("\nDespués de ordenar el arreglo: [" + string.Join(", ", lista) + "]");
    }
}
