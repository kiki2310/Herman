#include <iostream>

void burbuja(int arr[], int n) {
    int i, j, temp;
    for (i = 0; i < n - 1; i++) {
        for (j = 0; j < n - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
            }
        }
    }
}
int main() {
    int a[5] = {42, 23, 17, 13, 5};
    burbuja(a, 5);
    for (int i = 0; i < 5; i++) {
        std::cout << a[i] << " ";
    }
    return 0;
}