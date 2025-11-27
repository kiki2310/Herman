#include <iostream>

template <typename T>
class NodoDoble {
public:
    T data;
    NodoDoble* next;
    NodoDoble* prev;

    NodoDoble(T data) {
        this->data = data;
        this->next = nullptr;
        this->prev = nullptr;
    }
};

template <typename T>
class ListaDoblementeEnlazada {
private:
    NodoDoble<T>* head;
    NodoDoble<T>* tail;

public:
    ListaDoblementeEnlazada() {
        this->head = nullptr;
        this->tail = nullptr;
    }

    void insertarAlPrincipio(T data) {
        NodoDoble<T>* nuevoNodo = new NodoDoble<T>(data);
        if (this->head == nullptr) {
            this->head = nuevoNodo;
            this->tail = nuevoNodo;
        } else {
            nuevoNodo->next = this->head;
            this->head->prev = nuevoNodo;
            this->head = nuevoNodo;
        }
    }

    void insertarAlFinal(T data) {
        NodoDoble<T>* nuevoNodo = new NodoDoble<T>(data);
        if (this->head == nullptr) {
            this->head = nuevoNodo;
            this->tail = nuevoNodo;
        } else {
            this->tail->next = nuevoNodo;
            nuevoNodo->prev = this->tail;
            this->tail = nuevoNodo;
        }
    }

    void imprimirLista() {
        NodoDoble<T>* nodoActual = this->head;
        while (nodoActual != nullptr) {
            std::cout << nodoActual->data << " <-> ";
            nodoActual = nodoActual->next;
        }
        std::cout << "None" << std::endl;
    }

    bool buscar(T dataBuscada) {
        NodoDoble<T>* nodoActual = this->head;
        while (nodoActual != nullptr) {
            if (nodoActual->data == dataBuscada) {
                return true;
            }
            nodoActual = nodoActual->next;
        }
        return false;
    }

    void eliminar(T dataAEliminar) {
        NodoDoble<T>* nodoActual = this->head;
        while (nodoActual != nullptr) {
            if (nodoActual->data == dataAEliminar) {
                break;
            }
            nodoActual = nodoActual->next;
        }

        if (nodoActual == nullptr) {
            return;
        }

        if (nodoActual->prev != nullptr) {
            nodoActual->prev->next = nodoActual->next;
        } else {
            this->head = nodoActual->next;
        }

        if (nodoActual->next != nullptr) {
            nodoActual->next->prev = nodoActual->prev;
        } else {
            this->tail = nodoActual->prev;
        }
        
        delete nodoActual;
    }
};

int main() {
    ListaDoblementeEnlazada<int> miLista;
    miLista.insertarAlFinal(10);
    miLista.insertarAlFinal(20);
    miLista.insertarAlFinal(30);
    miLista.imprimirLista();

    miLista.insertarAlPrincipio(5);
    miLista.imprimirLista();

    std::cout << "¿Está el 20? " << (miLista.buscar(20) ? "True" : "False") << std::endl;
    std::cout << "¿Está el 99? " << (miLista.buscar(99) ? "True" : "False") << std::endl;

    miLista.eliminar(20);
    miLista.imprimirLista();

    miLista.eliminar(5);
    miLista.imprimirLista();

    miLista.eliminar(30);
    miLista.imprimirLista();

    return 0;
}