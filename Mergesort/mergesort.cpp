#include <iostream>

// Función para imprimir un arreglo
void printArray(int A[], int size) {
    for (int i = 0; i < size; i++)
        std::cout << A[i] << " ";
    std::cout << std::endl;
}

// Combina dos subarreglos de arr[].
void merge(int arr[], int l, int m, int r) {
    int n1 = m - l + 1;
    int n2 = r - m;

    // Crea arreglos temporales
    int* L = new int[n1];
    int* R = new int[n2];

    // Copia los datos a los arreglos temporales L[] y R[]
    for (int i = 0; i < n1; i++)
        L[i] = arr[l + i];
    for (int j = 0; j < n2; j++)
        R[j] = arr[m + 1 + j];

    int i = 0; // Índice inicial del primer subarreglo
    int j = 0; // Índice inicial del segundo subarreglo
    int k = l; // Índice inicial del subarreglo combinado

    // Combina los arreglos temporales de nuevo en arr[l..r]
    while (i < n1 && j < n2) {
        if (L[i] <= R[j]) {
            arr[k] = L[i];
            i++;
        } else {
            arr[k] = R[j];
            j++;
        }
        k++;
    }

    // Copia los elementos restantes de L[], si hay alguno
    while (i < n1) {
        arr[k] = L[i];
        i++;
        k++;
    }

    // Copia los elementos restantes de R[], si hay alguno
    while (j < n2) {
        arr[k] = R[j];
        j++;
        k++;
    }
    
    // Libera la memoria de los arreglos temporales
    delete[] L;
    delete[] R;
}

// l es para el índice izquierdo y r es para el índice derecho del subarreglo de arr a ser ordenado
void mergeSort(int arr[], int l, int r) {
    if (l < r) {
        // Igual que (l+r)/2, pero evita desbordamiento para l y r grandes
        int m = l + (r - l) / 2;

        // Ordena la primera y segunda mitad
        mergeSort(arr, l, m);
        mergeSort(arr, m + 1, r);

        merge(arr, l, m, r);
    }
}

// Código de ejecución
int main() {
    int arr[] = { 39, 28, 44, 11 };
    int arr_size = sizeof(arr) / sizeof(arr[0]);

    std::cout << "Arreglo antes de ordenar: \n";
    printArray(arr, arr_size);

    mergeSort(arr, 0, arr_size - 1);

    std::cout << "\nArreglo después de ordenar: \n";
    printArray(arr, arr_size);
    return 0;
}
