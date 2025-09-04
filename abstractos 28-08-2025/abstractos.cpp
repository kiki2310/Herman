#include <iostream>
#include <string>

class Mascota {
public:
    std::string nombre;
    std::string tipo;
    int edad;
    std::string raza;

    Mascota(std::string n, std::string t, int e, std::string r) {
        nombre = n;
        tipo = t;
        edad = e;
        raza = r;
    }
};

int main() {
    Mascota cat("Whiskers", "Gato", 3, "Siames");
    std::cout << "Nombre: " << cat.nombre << ", Tipo: " << cat.tipo << ", Edad: " << cat.edad << ", Raza: " << cat.raza << std::endl;
    return 0;
}
