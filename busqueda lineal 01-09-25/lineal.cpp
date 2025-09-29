#include <iostream>

using namespace std;
void imprimirArreglo(int arr[], int size) {
    cout << "Contenido del arreglo: ";
    for (int i = 0; i < size; i++) {
        cout << arr[i] << " ";
    }
    cout << endl;
}
int busquedaLineal(int arr[], int size, int objetivo) {
    for (int i = 0; i < size; i++) {
        if (arr[i] == objetivo) {
            return i; // Retorna el índice si se encuentra el elemento
        }
    }
    return -1; // Retorna -1 si no se encuentra el elemento
}
void insertarnumero(int arr[], int nuevoValor, int posicion, int size) {

    if (posicion < 0 || posicion > size) {
        cout << "Posición inválida." << endl;
        return;
    }
    for (int i = size - 1; i > posicion; i--) {
        arr[i] = arr[i - 1];
    }
    arr[posicion] = nuevoValor;
}

int main() {
    int arreglo[100] = {10, 20, 30, 40, 50};
    int size = 5;
    imprimirArreglo(arreglo, size);

    int objetivo;
    cout << "Introduce el elemento a buscar: ";
    cin >> objetivo;

    int indice = busquedaLineal(arreglo, size, objetivo);
    if (indice != -1) {
        cout << "Elemento encontrado en el índice: " << indice << endl;
    } else {
        cout << "Elemento no encontrado." << endl;
    }
    int nuevoValor, posicion;
    cout << "Valor para insertar: ";
    cin >> nuevoValor;
    cout << "Posición para insertar: ";
    cin >> posicion;
    insertarnumero(arreglo, nuevoValor, posicion, size);
    imprimirArreglo(arreglo, size);
    cout << "Fin del programa." << endl;

    return 0;
}