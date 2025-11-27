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
class ListaDoblementeCircular {
private:
    NodoDoble<T>* head;

public:
    ListaDoblementeCircular() {
        this->head = nullptr;
    }

    void insertarAlPrincipio(T data) {
        NodoDoble<T>* nuevoNodo = new NodoDoble<T>(data);
        if (this->head == nullptr) {
            this->head = nuevoNodo;
            nuevoNodo->next = this->head;
            nuevoNodo->prev = this->head;
        } else {
            NodoDoble<T>* tail = this->head->prev;
            nuevoNodo->next = this->head;
            this->head->prev = nuevoNodo;
            this->head = nuevoNodo;
            this->head->prev = tail;
            tail->next = this->head;
        }
    }

    void insertarAlFinal(T data) {
        NodoDoble<T>* nuevoNodo = new NodoDoble<T>(data);
        if (this->head == nullptr) {
            this->head = nuevoNodo;
            nuevoNodo->next = this->head;
            nuevoNodo->prev = this->head;
        } else {
            NodoDoble<T>* tail = this->head->prev;
            tail->next = nuevoNodo;
            nuevoNodo->prev = tail;
            nuevoNodo->next = this->head;
            this->head->prev = nuevoNodo;
        }
    }

    void imprimirLista() {
        if (this->head == nullptr) {
            std::cout << "None" << std::endl;
            return;
        }

        NodoDoble<T>* nodoActual = this->head;
        do {
            std::cout << nodoActual->data << " <-> ";
            nodoActual = nodoActual->next;
        } while (nodoActual != this->head);
        std::cout << "(vuelve a " << this->head->data << ")" << std::endl;
    }

    bool buscar(T dataBuscada) {
        if (this->head == nullptr) {
            return false;
        }

        NodoDoble<T>* nodoActual = this->head;
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

        NodoDoble<T>* nodoActual = this->head;
        bool encontrado = false;
        do {
            if (nodoActual->data == dataAEliminar) {
                encontrado = true;
                break;
            }
            nodoActual = nodoActual->next;
        } while (nodoActual != this->head);

        if (!encontrado) return;

        if (nodoActual == this->head && this->head->next == this->head) {
            delete this->head;
            this->head = nullptr;
            return;
        }

        if (nodoActual == this->head) {
            this->head = nodoActual->next;
        }

        nodoActual->prev->next = nodoActual->next;
        nodoActual->next->prev = nodoActual->prev;
        delete nodoActual;
    }
};

int main() {
    ListaDoblementeCircular<int> miLista;
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