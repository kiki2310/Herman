#include <iostream>

// Función de ordenamiento por inserción para arreglos
void insertionSort(double bukt[], int n) { // 'n' es el tamaño real del bucket
    for (int j = 1; j < n; ++j) {
        double val = bukt[j];
        int k = j - 1;
        while (k >= 0 && bukt[k] > val) {
            bukt[k + 1] = bukt[k];
            k--;
        }
        bukt[k + 1] = val;
    }
}

// Función principal de ordenamiento bucket con arreglos
void bucketSort(double inputArr[], int s) { // 's' es el tamaño del arreglo
    if (s <= 0) return;

    // 1. Crear los buckets (arreglo de punteros a arreglos)
    double** bucketArr = new double*[s];
    
    // Arreglo para guardar los tamaños actuales de cada bucket
    int* bucketSizes = new int[s];

    // Inicializar cada bucket
    for (int i = 0; i < s; i++) {
        // Asignamos memoria para el peor caso (todos en un bucket)
        bucketArr[i] = new double[s]; 
        bucketSizes[i] = 0; // Empezamos con 0 elementos
    }

    // 2. Colocar cada elemento en su bucket
    for (int i = 0; i < s; i++) {
        double j = inputArr[i];
        int bi = s * j;
        if (bi >= s) bi = s - 1; // Asegurar que 1.0 caiga en el último bucket

        // Añadir elemento 'j' al bucket 'bi'
        bucketArr[bi][bucketSizes[bi]] = j;
        bucketSizes[bi]++; // Incrementar el contador de ese bucket
    }

    // 3. Ordenar cada bucket individualmente
    for (int i = 0; i < s; i++) {
        insertionSort(bucketArr[i], bucketSizes[i]); // Pasamos el tamaño real
    }

    // 4. Concatenar todos los buckets en el arreglo original
    int idx = 0;
    for (int i = 0; i < s; i++) {
        for (int j = 0; j < bucketSizes[i]; j++) {
            inputArr[idx++] = bucketArr[i][j];
        }
    }

    // 5. ¡Importante! Liberar la memoria que pedimos con 'new'
    for (int i = 0; i < s; i++) {
        delete[] bucketArr[i]; // Borrar cada sub-arreglo
    }
    delete[] bucketArr;     // Borrar el arreglo de punteros
    delete[] bucketSizes;  // Borrar el arreglo de tamaños
}

// Función para imprimir el arreglo
void printArray(double arr[], int n) {
    for (int i = 0; i < n; i++) {
        std::cout << arr[i] << " ";
    }
    std::cout << std::endl;
}

int main() {
    double inputArr[] = {0.77, 0.16, 0.38, 0.25, 0.71, 0.93, 0.22, 0.11, 0.24, 0.67};
    // Calcular el tamaño del arreglo
    int s = sizeof(inputArr) / sizeof(inputArr[0]);

    std::cout << "Arreglo antes de ordenar:" << std::endl;
    printArray(inputArr, s);

    bucketSort(inputArr, s);

    std::cout << "Arreglo después de ordenar:" << std::endl;
    printArray(inputArr, s);

    return 0;
}