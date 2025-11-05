#include <iostream>
#include <vector>

class ShellSort {
public:
    // Una función de utilidad para imprimir un vector
    static void displayArr(const std::vector<int>& inputArr) {
        for (int k : inputArr) {
            std::cout << k << " ";
        }
        std::cout << std::endl;
    }

    // Función para ordenar inputArr usando Shell Sort
    void sort(std::vector<int>& inputArr) {
        int size = inputArr.size();
        // Comienza con un espacio grande y luego reduce el espacio
        for (int gapSize = size / 2; gapSize > 0; gapSize /= 2) {
            
            // Realiza un ordenamiento por inserción con espacios
            for (int j = gapSize; j < size; j++) {
                int val = inputArr[j];
                int k = j;

                // Compara y mueve elementos mientras el anterior (a una distancia 'gapSize') sea mayor
                while (k >= gapSize && inputArr[k - gapSize] > val) {
                    inputArr[k] = inputArr[k - gapSize];
                    k -= gapSize;
                }

                // Inserta el elemento en su posición correcta
                inputArr[k] = val;
            }
        }
    }
};

// Bloque principal
int main() {
    std::vector<int> inputArr = {36, 34, 43, 11, 15, 20, 28, 45};
    std::cout << "Arreglo antes de ser ordenado:" << std::endl;
    ShellSort::displayArr(inputArr);

    ShellSort obj;
    obj.sort(inputArr);

    std::cout << "Arreglo después de ser ordenado:" << std::endl;
    ShellSort::displayArr(inputArr);

    return 0;
}
