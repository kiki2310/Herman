class ColaArreglo {
    constructor(tamanoMax) {
        this.maxSize = tamanoMax;
        this.queue = new Array(tamanoMax);
        this.front = -1;
        this.rear = -1;
    }

    isFull() {
        return (this.front === 0 && this.rear === this.maxSize - 1) || (this.front === this.rear + 1);
    }

    isEmpty() {
        return this.front === -1;
    }

    enqueue(data) {
        if (this.isFull()) {
            console.log("Error: Cola desbordada (Overflow)");
            return;
        }

        if (this.front === -1) {
            this.front = 0;
        }

        this.rear = (this.rear + 1) % this.maxSize;
        this.queue[this.rear] = data;
    }

    dequeue() {
        if (this.isEmpty()) {
            console.log("Error: Cola subdesbordada (Underflow)");
            return null;
        }

        const data = this.queue[this.front];
        if (this.front === this.rear) {
            this.front = -1;
            this.rear = -1;
        } else {
            this.front = (this.front + 1) % this.maxSize;
        }
        return data;
    }

    peek() {
        if (this.isEmpty()) {
            console.log("Cola vacía");
            return null;
        }
        return this.queue[this.front];
    }
}

const cola = new ColaArreglo(5);
cola.enqueue(10);
cola.enqueue(20);
cola.enqueue(30);

console.log(`Elemento frontal: ${cola.peek()}`);
console.log(`Elimina elemento: ${cola.dequeue()}`);
console.log(`Nuevo elemento frontal: ${cola.peek()}`);

cola.enqueue(40);
cola.enqueue(50);
cola.enqueue(60);
console.log(`Elimina elemento: ${cola.dequeue()}`);