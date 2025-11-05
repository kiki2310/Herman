using System;
using System.Collections.Generic;

public class Radixsort
{
    public static void Sort(int[] array)
    {
        int max = GetMax(array);
        for (int exp = 1; max / exp > 0; exp *= 10)
        {
            CountingSortByDigit(array, exp);
        }
    }

    private static int GetMax(int[] array)
    {
        int max = array[0];
        foreach (int num in array)
        {
            if (num > max)
            {
                max = num;
            }
        }
        return max;
    }

    private static void CountingSortByDigit(int[] array, int exp)
    {
        int n = array.Length;
        int[] output = new int[n];
        int[] count = new int[10];

        for (int i = 0; i < n; i++)
        {
            int digit = (array[i] / exp) % 10;
            count[digit]++;
        }

        for (int i = 1; i < 10; i++)
        {
            count[i] += count[i - 1];
        }

        for (int i = n - 1; i >= 0; i--)
        {
            int digit = (array[i] / exp) % 10;
            output[count[digit] - 1] = array[i];
            count[digit]--;
        }

        for (int i = 0; i < n; i++)
        {
            array[i] = output[i];
        }

    }
    public static void Main(string[] args)
    {
        int[] array = { 170, 45, 75, 90, 802, 24, 2, 66 };
        Console.WriteLine("Original array: " + string.Join(", ", array));
        Sort(array);
        Console.WriteLine("Sorted array: " + string.Join(", ", array));
    }
}

