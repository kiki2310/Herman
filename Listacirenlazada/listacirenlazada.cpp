#include <iostream>

template <typename T>
class Nodo {
public:
    T data;
    Nodo* next;

    Nodo(T data) {
        this->data = data;
        this->next = nullptr;
    }
};

template <typename T>
class ListaCircularEnlazada {
private:
    Nodo<T>* head;

public:
    ListaCircularEnlazada() {
        this->head = nullptr;
    }

    void insertarAlPrincipio(T data) {
        Nodo<T>* nuevoNodo = new Nodo<T>(data);
        if (this->head == nullptr) {
            this->head = nuevoNodo;
            nuevoNodo->next = this->head;
        } else {
            Nodo<T>* nodoActual = this->head;
            while (nodoActual->next != this->head) {
                nodoActual = nodoActual->next;
            }
            nuevoNodo->next = this->head;
            this->head = nuevoNodo;
            nodoActual->next = this->head;
        }
    }

    void insertarAlFinal(T data) {
        Nodo<T>* nuevoNodo = new Nodo<T>(data);
        if (this->head == nullptr) {
            this->head = nuevoNodo;
            nuevoNodo->next = this->head;
        } else {
            Nodo<T>* nodoActual = this->head;
            while (nodoActual->next != this->head) {
                nodoActual = nodoActual->next;
            }
            nodoActual->next = nuevoNodo;
            nuevoNodo->next = this->head;
        }
    }

    void imprimirLista() {
        if (this->head == nullptr) {
            std::cout << "None" << std::endl;
            return;
        }

        Nodo<T>* nodoActual = this->head;
        do {
            std::cout << nodoActual->data << " -> ";
            nodoActual = nodoActual->next;
        } while (nodoActual != this->head);
        std::cout << "(vuelve a " << this->head->data << ")" << std::endl;
    }

    bool buscar(T dataBuscada) {
        if (this->head == nullptr) {
            return false;
        }

        Nodo<T>* nodoActual = this->head;
        do {
            if (nodoActual->data == dataBuscada) {
                return true;
            }
            nodoActual = nodoActual->next;
        } while (nodoActual != this->head);
        return false;
    }

    void eliminar(T dataAEliminar) {
        if (this->head == nullptr) {
            return;
        }

        if (this->head->data == dataAEliminar) {
            if (this->head->next == this->head) {
                delete this->head;
                this->head = nullptr;
                return;
            }

            Nodo<T>* nodoActual = this->head;
            while (nodoActual->next != this->head) {
                nodoActual = nodoActual->next;
            }

            Nodo<T>* temp = this->head;
            nodoActual->next = this->head->next;
            this->head = this->head->next;
            delete temp;
            return;
        }

        Nodo<T>* nodoPrevio = this->head;
        Nodo<T>* nodoActual = this->head->next;
        while (nodoActual != this->head) {
            if (nodoActual->data == dataAEliminar) {
                nodoPrevio->next = nodoActual->next;
                delete nodoActual;
                return;
            }
            nodoPrevio = nodoActual;
            nodoActual = nodoActual->next;
        }
    }
};

int main() {
    ListaCircularEnlazada<int> miLista;
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