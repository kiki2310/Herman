class Nodo {
    constructor(data) {
        this.data = data;
        this.next = null;
    }
}

class PilaLista {
    constructor() {
        this.top = null;
    }

    isEmpty() {
        return this.top === null;
    }

    push(data) {
        const nuevoNodo = new Nodo(data);
        nuevoNodo.next = this.top;
        this.top = nuevoNodo;
    }

    pop() {
        if (this.isEmpty()) {
            console.log("Error: Stack Underflow");
            return null;
        }
        const data = this.top.data;
        this.top = this.top.next;
        return data;
    }

    peek() {
        if (this.isEmpty()) {
            console.log("Pila vacía");
            return null;
        }
        return this.top.data;
    }
}

const pila = new PilaLista();
pila.push(10);
pila.push(20);
pila.push(30);

console.log(`Elemento superior: ${pila.peek()}`);
console.log(`Extrae elemento: ${pila.pop()}`);
console.log(`Nuevo elemento superior: ${pila.peek()}`);