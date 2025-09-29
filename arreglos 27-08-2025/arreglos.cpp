
#include <iostream>
#include <vector> // Necesario para usar std::vector

int main() {
    // 1. Declaración e inicialización
    // Un vector vacío de enteros
    std::vector<int> numeros;

    // Un vector con elementos iniciales
    std::vector<int> numeros_inicializados = {10, 20, 30, 40, 50};
    std::cout << "Vector inicial: ";
    for (int num : numeros_inicializados) {
        std::cout << num << " ";
    }
    std::cout << std::endl;

    // 2. Acceder a elementos
    // Los índices comienzan en 0
    int primerElemento = numeros_inicializados[0]; // Accede al 10
    std::cout << "El primer elemento es: " << primerElemento << std::endl;

    // 3. Modificar elementos
    numeros_inicializados[2] = 35; // Cambia el tercer elemento (30) a 35

    // 4. Añadir elementos
    // .push_back() añade un elemento al final
    numeros_inicializados.push_back(60);

    // 5. Tamaño del vector
    std::cout << "El tamaño del vector es: " << numeros_inicializados.size() << std::endl;

    // 6. Iterar sobre un vector (usando un bucle for basado en rango)
    std::cout << "Vector final: ";
    for (int numero : numeros_inicializados) {
        std::cout << numero << " ";
    }
    std::cout << std::endl;

    return 0;
}

