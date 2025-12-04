#include <iostream>
#include <vector>

class PilaArreglo {
private:
    std::vector<int> stack;
    int maxSize;
    int top;

public:
    PilaArreglo(int tamanoMax) {
        this->maxSize = tamanoMax;
        this->stack.resize(tamanoMax);
        this->top = -1;
    }

    bool isEmpty() {
        return this->top == -1;
    }

    bool isFull() {
        return this->top == this->maxSize - 1;
    }

    void push(int data) {
        if (this->isFull()) {
            std::cout << "Error: Stack Overflow" << std::endl;
            return;
        }
        this->top++;
        this->stack[this->top] = data;
    }

    int pop() {
        if (this->isEmpty()) {
            std::cout << "Error: Stack Underflow" << std::endl;
            return -1;
        }
        int data = this->stack[this->top];
        this->top--;
        return data;
    }

    int peek() {
        if (this->isEmpty()) {
            std::cout << "Pila vacía" << std::endl;
            return -1;
        }
        return this->stack[this->top];
    }
};

int main() {
    PilaArreglo pila(100);
    pila.push(10);
    pila.push(20);
    pila.push(30);

    std::cout << "Elemento superior: " << pila.peek() << std::endl;
    std::cout << "Extrae elemento: " << pila.pop() << std::endl;
    std::cout << "Nuevo elemento superior: " << pila.peek() << std::endl;

    return 0;
}