#include <iostream>

class Nodo {
public:
    int data;
    Nodo* next;

    Nodo(int data) {
        this->data = data;
        this->next = nullptr;
    }
};

class PilaLista {
private:
    Nodo* top;

public:
    PilaLista() {
        this->top = nullptr;
    }

    bool isEmpty() {
        return this->top == nullptr;
    }

    void push(int data) {
        Nodo* nuevoNodo = new Nodo(data);
        nuevoNodo->next = this->top;
        this->top = nuevoNodo;
    }

    int pop() {
        if (this->isEmpty()) {
            std::cout << "Error: Stack Underflow" << std::endl;
            return -1;
        }
        int data = this->top->data;
        Nodo* temp = this->top;
        this->top = this->top->next;
        delete temp;
        return data;
    }

    int peek() {
        if (this->isEmpty()) {
            std::cout << "Pila vacía" << std::endl;
            return -1;
        }
        return this->top->data;
    }
};

int main() {
    PilaLista pila;
    pila.push(10);
    pila.push(20);
    pila.push(30);

    std::cout << "Elemento superior: " << pila.peek() << std::endl;
    std::cout << "Extrae elemento: " << pila.pop() << std::endl;
    std::cout << "Nuevo elemento superior: " << pila.peek() << std::endl;

    return 0;
}