#include <iostream>
#include <vector>
#include <algorithm> // Para std::swap

void burbuja(int arr[], int n) {
    int i, j, temp;
    for (i = 0; i < n - 1; i++) {
        for (j = 0; j < n - i - 1; j++) {
// Usar std::vector es más seguro y flexible que los arreglos C-style.
// La función toma el vector por referencia (&) para modificar el original.
void bubbleSort(std::vector<int>& arr) {
    size_t n = arr.size();
    bool swapped;
    for (size_t i = 0; i < n - 1; i++) {
        swapped = false;
        for (size_t j = 0; j < n - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
                // std::swap es la forma idiomática de intercambiar valores.
                std::swap(arr[j], arr[j + 1]);
                swapped = true;
            }
        }
        // Optimización: si no hubo intercambios, el arreglo ya está ordenado.
        if (!swapped) {
            break;
        }
    }
}
int main() {
    int a[5] = {42, 23, 17, 13, 5};
    burbuja(a, 5);
    for (int i = 0; i < 5; i++) {
        std::cout << a[i] << " ";
    std::vector<int> data = {42, 23, 17, 13, 5};
    bubbleSort(data);
    // Un bucle for-each es más limpio para imprimir los elementos.
    for (int num : data) {
        std::cout << num << " ";
    }
    std::cout << std::endl;
    return 0;
}