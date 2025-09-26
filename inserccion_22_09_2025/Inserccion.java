public class Inserccion {
    public static void main(String[] args) {
        int[] arr = {5, 2, 9, 1, 5, 6};
        insertionSort(arr);
        for (int num : arr) {
            System.out.print(num + " ");
        }
    }
    public static void insertionSort(int[] arr) {
        for (int i = 1; i < arr.length; i++) {
            int tem = arr[i];
            int j = i - 1;
            while (j >= 0 && arr[j] > tem) {
                arr[j + 1] = arr[j];
                j--;
            }
            arr[j + 1] = tem;
        }
    }
}