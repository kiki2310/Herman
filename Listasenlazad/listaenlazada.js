class Nodo {
    constructor(data) {
        this.data = data;
        this.next = null;
    }
}

class ListaEnlazada {
    constructor() {
        this.head = null;
    }

    insertarAlPrincipio(data) {
        const nuevoNodo = new Nodo(data);
        nuevoNodo.next = this.head;
        this.head = nuevoNodo;
    }

    insertarAlFinal(data) {
        const nuevoNodo = new Nodo(data);
        if (!this.head) {
            this.head = nuevoNodo;
            return;
        }

        let ultimoNodo = this.head;
        while (ultimoNodo.next) {
            ultimoNodo = ultimoNodo.next;
        }
        
        ultimoNodo.next = nuevoNodo;
    }

    imprimirLista() {
        let nodoActual = this.head;
        let resultado = "";
        while (nodoActual) {
            resultado += `${nodoActual.data} -> `;
            nodoActual = nodoActual.next;
        }
        console.log(resultado + "None");
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
        let nodoPrevio = null;

        if (nodoActual && nodoActual.data === dataAEliminar) {
            this.head = nodoActual.next;
            return;
        }

        while (nodoActual && nodoActual.data !== dataAEliminar) {
            nodoPrevio = nodoActual;
            nodoActual = nodoActual.next;
        }

        if (!nodoActual) {
            return;
        }

        nodoPrevio.next = nodoActual.next;
    }
}

// --- Demostración ---
const miLista = new ListaEnlazada();
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
