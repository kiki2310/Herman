class PilaArreglo {
    constructor(tamanoMax) {
        this.stack = new Array(tamanoMax);
        this.maxSize = tamanoMax;
        this.top = -1;
    }

    isEmpty() {
        return this.top === -1;
    }

    isFull() {
        return this.top === this.maxSize - 1;
    }

    push(data) {
        if (this.isFull()) {
            console.log("Error: Stack Overflow");
            return;
        }
        this.top++;
        this.stack[this.top] = data;
    }

    pop() {
        if (this.isEmpty()) {
            console.log("Error: Stack Underflow");
            return -1;
        }
        const data = this.stack[this.top];
        this.top--;
        return data;
    }

    peek() {
        if (this.isEmpty()) {
            console.log("Pila vacía");
            return -1;
        }
        return this.stack[this.top];
    }
}

const pila = new PilaArreglo(100);
pila.push(10);
pila.push(20);
pila.push(30);

console.log(`Elemento superior: ${pila.peek()}`);
console.log(`Extrae elemento: ${pila.pop()}`);
console.log(`Nuevo elemento superior: ${pila.peek()}`);