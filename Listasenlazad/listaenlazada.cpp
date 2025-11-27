#include <iostream>

template<typename T>
class Nodo {
public:
    T data;
    Nodo<T>* next;

    Nodo(T data) : data(data), next(nullptr) {}
};

template<typename T>
class ListaEnlazada {
private:
    Nodo<T>* head;

public:
    ListaEnlazada() : head(nullptr) {}

    ~ListaEnlazada() {
        Nodo<T>* actual = head;
        Nodo<T>* siguiente = nullptr;
        while (actual != nullptr) {
            siguiente = actual->next;
            delete actual;
            actual = siguiente;
        }
        head = nullptr;
    }

    void insertarAlPrincipio(T data) {
        Nodo<T>* nuevoNodo = new Nodo<T>(data);
        nuevoNodo->next = head;
        head = nuevoNodo;
    }

    void insertarAlFinal(T data) {
        Nodo<T>* nuevoNodo = new Nodo<T>(data);
        if (head == nullptr) {
            head = nuevoNodo;
            return;
        }

        Nodo<T>* ultimoNodo = head;
        while (ultimoNodo->next != nullptr) {
            ultimoNodo = ultimoNodo->next;
        }
        
        ultimoNodo->next = nuevoNodo;
    }

    void imprimirLista() {
        Nodo<T>* nodoActual = head;
        while (nodoActual != nullptr) {
            std::cout << nodoActual->data << " -> ";
            nodoActual = nodoActual->next;
        }
        std::cout << "None" << std::endl;
    }

    bool buscar(T dataBuscada) {
        Nodo<T>* nodoActual = head;
        while (nodoActual != nullptr) {
            if (nodoActual->data == dataBuscada) {
                return true;
            }
            nodoActual = nodoActual->next;
        }
        return false;
    }

    void eliminar(T dataAEliminar) {
        Nodo<T>* nodoActual = head;
        Nodo<T>* nodoPrevio = nullptr;

        if (nodoActual != nullptr && nodoActual->data == dataAEliminar) {
            head = nodoActual->next;
            delete nodoActual;
            return;
        }

        while (nodoActual != nullptr && nodoActual->data != dataAEliminar) {
            nodoPrevio = nodoActual;
            nodoActual = nodoActual->next;
        }

        if (nodoActual == nullptr) {
            return;
        }

        nodoPrevio->next = nodoActual->next;
        delete nodoActual;
    }
};

int main() {
    ListaEnlazada<int> miLista;
    miLista.insertarAlFinal(10);
    miLista.insertarAlFinal(20);
    miLista.insertarAlFinal(30);
    miLista.imprimirLista();

    miLista.insertarAlPrincipio(5);
    miLista.imprimirLista();

    std::cout << "¿Está el 20? " << (miLista.buscar(20) ? "true" : "false") << std::endl;
    std::cout << "¿Está el 99? " << (miLista.buscar(99) ? "true" : "false") << std::endl;

    miLista.eliminar(20);
    miLista.imprimirLista();
    
    miLista.eliminar(5);
    miLista.imprimirLista();

    return 0;
}
