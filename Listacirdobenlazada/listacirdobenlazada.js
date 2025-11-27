class NodoDoble {
    constructor(data) {
        this.data = data;
        this.next = null;
        this.prev = null;
    }
}

class ListaDoblementeCircular {
    constructor() {
        this.head = null;
    }

    insertarAlPrincipio(data) {
        const nuevoNodo = new NodoDoble(data);
        if (this.head === null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
            nuevoNodo.prev = this.head;
        } else {
            const tail = this.head.prev;
            nuevoNodo.next = this.head;
            this.head.prev = nuevoNodo;
            this.head = nuevoNodo;
            this.head.prev = tail;
            tail.next = this.head;
        }
    }

    insertarAlFinal(data) {
        const nuevoNodo = new NodoDoble(data);
        if (this.head === null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
            nuevoNodo.prev = this.head;
        } else {
            const tail = this.head.prev;
            tail.next = nuevoNodo;
            nuevoNodo.prev = tail;
            nuevoNodo.next = this.head;
            this.head.prev = nuevoNodo;
        }
    }

    imprimirLista() {
        if (this.head === null) {
            console.log("None");
            return;
        }

        let nodoActual = this.head;
        let salida = "";
        do {
            salida += nodoActual.data + " <-> ";
            nodoActual = nodoActual.next;
        } while (nodoActual !== this.head);
        console.log(salida + `(vuelve a ${this.head.data})`);
    }

    buscar(dataBuscada) {
        if (this.head === null) {
            return false;
        }

        let nodoActual = this.head;
        do {
            if (nodoActual.data === dataBuscada) {
                return true;
            }
            nodoActual = nodoActual.next;
        } while (nodoActual !== this.head);
        return false;
    }

    eliminar(dataAEliminar) {
        if (this.head === null) {
            return;
        }

        let nodoActual = this.head;
        let encontrado = false;
        do {
            if (nodoActual.data === dataAEliminar) {
                encontrado = true;
                break;
            }
            nodoActual = nodoActual.next;
        } while (nodoActual !== this.head);

        if (!encontrado) return;

        if (nodoActual === this.head && this.head.next === this.head) {
            this.head = null;
            return;
        }

        if (nodoActual === this.head) {
            this.head = nodoActual.next;
        }

        nodoActual.prev.next = nodoActual.next;
        nodoActual.next.prev = nodoActual.prev;
    }
}

const miLista = new ListaDoblementeCircular();
miLista.insertarAlFinal(10);
miLista.insertarAlFinal(20);
miLista.insertarAlFinal(30);
miLista.imprimirLista();

miLista.insertarAlPrincipio(5);
miLista.imprimirLista();

console.log(`¿Está el 20? ${miLista.buscar(20)}`);
console.log(`¿Está el 99? ${miLista.buscar(99)}`);

miLista.eliminar(20);
miLista.imprimirLista();

miLista.eliminar(5);
miLista.imprimirLista();

miLista.eliminar(30);
miLista.imprimirLista();