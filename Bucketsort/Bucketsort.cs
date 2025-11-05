using System;
using System.Collections.Generic;
using System.Linq;

public class BucketSort
{
    // Función de ordenamiento por inserción
    public static void InsertionSort(List<double> bucket)
    {
        for (int j = 1; j < bucket.Count; j++)
        {
            double val = bucket[j];
            int k = j - 1;
            while (k >= 0 && bucket[k] > val)
            {
                bucket[k + 1] = bucket[k];
                k--;
            }
            bucket[k + 1] = val;
        }
    }

    // Función principal de ordenamiento bucket
    public static void Sort(double[] inputArr)
    {
        int s = inputArr.Length;
        List<List<double>> bucketArr = new List<List<double>>(s);
        for (int i = 0; i < s; i++)
        {
            bucketArr.Add(new List<double>());
        }

        // Colocar cada elemento en su bucket correspondiente
        foreach (double j in inputArr)
        {
            int bi = (int)(s * j);
            if (bi >= s) bi = s - 1; // Asegurar que el índice no se salga
            bucketArr[bi].Add(j);
        }

        // Ordenar cada bucket individualmente
        foreach (List<double> bukt in bucketArr)
        {
            InsertionSort(bukt);
        }

        // Concatenar todos los buckets en el arreglo original
        int idx = 0;
        foreach (List<double> bukt in bucketArr)
        {
            foreach (double j in bukt)
            {
                inputArr[idx++] = j;
            }
        }
    }

    public static void Main(string[] args)
    {
        double[] inputArr = { 0.77, 0.16, 0.38, 0.25, 0.71, 0.93, 0.22, 0.11, 0.24, 0.67 };

        Console.WriteLine("Arreglo antes de ordenar:");
        Console.WriteLine(string.Join(" ", inputArr));

        Sort(inputArr);

        Console.WriteLine("Arreglo después de ordenar:");
        Console.WriteLine(string.Join(" ", inputArr));
    }
}