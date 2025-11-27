class NodoDoble {
    constructor(data) {
        this.data = data;
        this.next = null;
        this.prev = null;
    }
}

class ListaDoblementeEnlazada {
    constructor() {
        this.head = null;
        this.tail = null;
    }

    insertarAlPrincipio(data) {
        const nuevoNodo = new NodoDoble(data);
        if (this.head === null) {
            this.head = nuevoNodo;
            this.tail = nuevoNodo;
        } else {
            nuevoNodo.next = this.head;
            this.head.prev = nuevoNodo;
            this.head = nuevoNodo;
        }
    }

    insertarAlFinal(data) {
        const nuevoNodo = new NodoDoble(data);
        if (this.head === null) {
            this.head = nuevoNodo;
            this.tail = nuevoNodo;
        } else {
            this.tail.next = nuevoNodo;
            nuevoNodo.prev = this.tail;
            this.tail = nuevoNodo;
        }
    }

    imprimirLista() {
        let nodoActual = this.head;
        let salida = "";
        while (nodoActual) {
            salida += nodoActual.data + " <-> ";
            nodoActual = nodoActual.next;
        }
        console.log(salida + "None");
    }

    buscar(dataBuscada) {
        let nodoActual = this.head;
        while (nodoActual) {
            if (nodoActual.data === dataBuscada) {
                return true;
            }
            nodoActual = nodoActual.next;
        }
        return false;
    }

    eliminar(dataAEliminar) {
        let nodoActual = this.head;
        while (nodoActual) {
            if (nodoActual.data === dataAEliminar) {
                break;
            }
            nodoActual = nodoActual.next;
        }

        if (nodoActual === null) {
            return;
        }

        if (nodoActual.prev) {
            nodoActual.prev.next = nodoActual.next;
        } else {
            this.head = nodoActual.next;
        }

        if (nodoActual.next) {
            nodoActual.next.prev = nodoActual.prev;
        } else {
            this.tail = nodoActual.prev;
        }
    }
}

const miLista = new ListaDoblementeEnlazada();
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