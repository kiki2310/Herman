#include <iostream>
#include <string>
using namespace std;

int main()
{
    char op = 's';
    string nombre;
    float altura;
    float peso;
    float imc;

    do {
        cout << "Ingresa el nombre de la persona: ";
        cin >> nombre;

        cout << "Ingresa la altura de la persona (metros): ";
        cin >> altura;

        if (altura <= 0) {
            cout << "Error: la altura debe ser mayor a 0\n";
            continue; 
        }

        cout << "Ingresa el peso de la persona (kilogramos): ";
        cin >> peso;

        imc = peso / (altura * altura);

        cout << "\n--- RESULTADOS ---\n";
        cout << "Nombre: " << nombre << endl;
        cout << "Peso: " << peso << " kg" << endl;
        cout << "Altura: " << altura << " m" << endl;
        cout << "IMC: " << imc << endl;

        if (imc < 18.5) {
            cout << "Composicion corporal: BAJO PESO\n";
        }
        else if (imc < 25) {
            cout << "Composicion corporal: NORMAL\n";
        }
        else if (imc < 30) {
            cout << "Composicion corporal: SOBREPESO\n";
        }
        else if (imc < 35) {
            cout << "Composicion corporal: OBESIDAD 1\n";
        }
        else if (imc < 40) {
            cout << "Composicion corporal: OBESIDAD 2\n";
        }
        else if (imc < 50) {
            cout << "Composicion corporal: OBESIDAD 3\n";
        }
        else {
            cout << "Composicion corporal: OBESIDAD 4\n";
        }

        cout << "\nDesea calcular el IMC de otra persona? (s/n): ";
        cin >> op;

    } while (op == 's' || op == 'S');

    return 0;
}
