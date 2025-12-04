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

class ColaLista {
private:
    Nodo* front;
    Nodo* rear;

public:
    ColaLista() {
        this->front = nullptr;
        this->rear = nullptr;
    }

    bool isEmpty() {
        return this->front == nullptr;
    }

    void enqueue(int data) {
        Nodo* nuevoNodo = new Nodo(data);
        if (this->rear == nullptr) {
            this->front = nuevoNodo;
            this->rear = nuevoNodo;
            return;
        }
        this->rear->next = nuevoNodo;
        this->rear = nuevoNodo;
    }

    int dequeue() {
        if (this->isEmpty()) {
            std::cout << "Error: Cola subdesbordada (Underflow)" << std::endl;
            return -1;
        }

        int data = this->front->data;
        Nodo* temp = this->front;
        this->front = this->front->next;

        if (this->front == nullptr) {
            this->rear = nullptr;
        }

        delete temp;
        return data;
    }

    int peek() {
        if (this->isEmpty()) {
            std::cout << "Cola vacía" << std::endl;
            return -1;
        }
        return this->front->data;
    }
};

int main() {
    ColaLista cola;
    cola.enqueue(10);
    cola.enqueue(20);
    cola.enqueue(30);

    std::cout << "Elemento frontal: " << cola.peek() << std::endl;
    std::cout << "Elimina elemento: " << cola.dequeue() << std::endl;
    std::cout << "Nuevo elemento frontal: " << cola.peek() << std::endl;

    cola.enqueue(40);
    std::cout << "Elimina elemento: " << cola.dequeue() << std::endl;
    std::cout << "Elimina elemento: " << cola.dequeue() << std::endl;
    std::cout << "Elimina elemento: " << cola.dequeue() << std::endl;
    std::cout << "Elimina elemento: " << cola.dequeue() << std::endl;

    return 0;
}