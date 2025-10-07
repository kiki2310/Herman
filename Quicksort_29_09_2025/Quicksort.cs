using System;

namespace Quicksort_29_09_2025
{
    public class Quicksort
    {
        public static void Main(string[] args)
        {
            int[] array = { 34, 7, 23, 32, 5, 62 };
            Console.WriteLine("Original array: " + string.Join(", ", array));

            Sort(array);

            Console.WriteLine("Sorted array: " + string.Join(", ", array));
        }
        public static void Sort(int[] array)
        {
            if (array == null || array.Length <= 1)
                return;

            QuickSort(array, 0, array.Length - 1);
        }

        private static void QuickSort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(array, low, high);
                QuickSort(array, low, pivotIndex - 1);
                QuickSort(array, pivotIndex + 1, high);
            }
        }

        private static int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (array[j] <= pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, high);
            return i + 1;
        }

        private static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}