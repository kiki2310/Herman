import java.util.ArrayList;
import java.util.List;
import java.util.Arrays;

public class Bucketsort {
    // Función de ordenamiento por inserción
    public static void insertionSort(List<Double> bucket) {
        for (int j = 1; j < bucket.size(); j++) {
            double val = bucket.get(j);
            int k = j - 1;
            while (k >= 0 && bucket.get(k) > val) {
                bucket.set(k + 1, bucket.get(k));
                k--;
            }
            bucket.set(k + 1, val);
        }
    }

    // Función principal de ordenamiento bucket
    public static void bucketSort(double[] inputArr) {
        int s = inputArr.length;
        if (s <= 0) return;

        // Crear lista de buckets vacíos
        List<List<Double>> bucketArr = new ArrayList<>(s);
        for (int i = 0; i < s; i++) {
            bucketArr.add(new ArrayList<>());
        }

        // Colocar cada elemento en su bucket
        for (double j : inputArr) {
            int bi = (int) (s * j);
            if (bi >= s) bi = s - 1; // Asegurar índice
            bucketArr.get(bi).add(j);
        }

        // Ordenar cada bucket
        for (List<Double> bukt : bucketArr) {
            insertionSort(bukt);
            // Alternativa más simple: Collections.sort(bukt);
        }

        // Concatenar los buckets en el arreglo original
        int idx = 0;
        for (List<Double> bukt : bucketArr) {
            for (double j : bukt) {
                inputArr[idx++] = j;
            }
        }
    }

    public static void main(String[] args) {
        double[] inputArr = {0.77, 0.16, 0.38, 0.25, 0.71, 0.93, 0.22, 0.11, 0.24, 0.67};

        System.out.println("Arreglo antes de ordenar:");
        System.out.println(Arrays.toString(inputArr));

        bucketSort(inputArr);

        System.out.println("Arreglo después de ordenar:");
        System.out.println(Arrays.toString(inputArr));
    }
}