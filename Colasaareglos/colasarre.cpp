#include <iostream>
#include <vector>

class ColaArreglo {
private:
    std::vector<int> queue;
    int maxSize;
    int front;
    int rear;

public:
    ColaArreglo(int tamanoMax) {
        this->maxSize = tamanoMax;
        this->queue.resize(tamanoMax);
        this->front = -1;
        this->rear = -1;
    }

    bool isFull() {
        return (this->front == 0 && this->rear == this->maxSize - 1) || (this->front == this->rear + 1);
    }

    bool isEmpty() {
        return this->front == -1;
    }

    void enqueue(int data) {
        if (this->isFull()) {
            std::cout << "Error: Cola desbordada (Overflow)" << std::endl;
            return;
        }

        if (this->front == -1) {
            this->front = 0;
        }

        this->rear = (this->rear + 1) % this->maxSize;
        this->queue[this->rear] = data;
    }

    int dequeue() {
        if (this->isEmpty()) {
            std::cout << "Error: Cola subdesbordada (Underflow)" << std::endl;
            return -1;
        }

        int data = this->queue[this->front];
        if (this->front == this->rear) {
            this->front = -1;
            this->rear = -1;
        } else {
            this->front = (this->front + 1) % this->maxSize;
        }
        return data;
    }

    int peek() {
        if (this->isEmpty()) {
            std::cout << "Cola vacía" << std::endl;
            return -1;
        }
        return this->queue[this->front];
    }
};

int main() {
    ColaArreglo cola(5);
    cola.enqueue(10);
    cola.enqueue(20);
    cola.enqueue(30);

    std::cout << "Elemento frontal: " << cola.peek() << std::endl;
    std::cout << "Elimina elemento: " << cola.dequeue() << std::endl;
    std::cout << "Nuevo elemento frontal: " << cola.peek() << std::endl;

    cola.enqueue(40);
    cola.enqueue(50);
    cola.enqueue(60);
    std::cout << "Elimina elemento: " << cola.dequeue() << std::endl;

    return 0;
}