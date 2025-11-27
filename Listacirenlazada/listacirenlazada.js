class Nodo {
    constructor(data) {
        this.data = data;
        this.next = null;
    }
}

class ListaCircularEnlazada {
    constructor() {
        this.head = null;
    }

    insertarAlPrincipio(data) {
        const nuevoNodo = new Nodo(data);
        if (this.head === null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
        } else {
            let nodoActual = this.head;
            while (nodoActual.next !== this.head) {
                nodoActual = nodoActual.next;
            }
            nuevoNodo.next = this.head;
            this.head = nuevoNodo;
            nodoActual.next = this.head;
        }
    }

    insertarAlFinal(data) {
        const nuevoNodo = new Nodo(data);
        if (this.head === null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
        } else {
            let nodoActual = this.head;
            while (nodoActual.next !== this.head) {
                nodoActual = nodoActual.next;
            }
            nodoActual.next = nuevoNodo;
            nuevoNodo.next = this.head;
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
            salida += nodoActual.data + " -> ";
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

        if (this.head.data === dataAEliminar) {
            if (this.head.next === this.head) {
                this.head = null;
                return;
            }

            let nodoActual = this.head;
            while (nodoActual.next !== this.head) {
                nodoActual = nodoActual.next;
            }

            nodoActual.next = this.head.next;
            this.head = this.head.next;
            return;
        }

        let nodoPrevio = this.head;
        let nodoActual = this.head.next;
        while (nodoActual !== this.head) {
            if (nodoActual.data === dataAEliminar) {
                nodoPrevio.next = nodoActual.next;
                return;
            }
            nodoPrevio = nodoActual;
            nodoActual = nodoActual.next;
        }
    }
}

const miLista = new ListaCircularEnlazada();
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