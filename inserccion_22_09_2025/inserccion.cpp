#include <iostream>

void inserccion(int arr[], int n) {
    int i, key, j;
    for (i = 1; i < n; i++) {
        key = arr[i];
        j = i - 1;

        // Mover los elementos de arr[0..i-1], que son mayores que key,
        // a una posición adelante de su posición actual
        while (j >= 0 && arr[j] > key) {
            arr[j + 1] = arr[j];
            j = j - 1;
        }
        arr[j + 1] = key;
    }
}
int main() {
    int a[5] = {42, 23, 17, 13, 5};
    inserccion(a, 5);
    for (int i = 0; i < 5; i++) {
        std::cout << a[i] << " ";
    }
    std::cout << std::endl;
    return 0;
}