public class Nodo<T> {
    public T data;
    public Nodo<T> next;

    public Nodo(T data) {
        this.data = data;
        this.next = null;
    }
}

public class ListaCircularEnlazada<T> {
    private Nodo<T> head;

    public ListaCircularEnlazada() {
        this.head = null;
    }

    public void insertarAlPrincipio(T data) {
        Nodo<T> nuevoNodo = new Nodo<>(data);
        if (this.head == null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
        } else {
            Nodo<T> nodoActual = this.head;
            while (nodoActual.next != this.head) {
                nodoActual = nodoActual.next;
            }
            nuevoNodo.next = this.head;
            this.head = nuevoNodo;
            nodoActual.next = this.head;
        }
    }

    public void insertarAlFinal(T data) {
        Nodo<T> nuevoNodo = new Nodo<>(data);
        if (this.head == null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
        } else {
            Nodo<T> nodoActual = this.head;
            while (nodoActual.next != this.head) {
                nodoActual = nodoActual.next;
            }
            nodoActual.next = nuevoNodo;
            nuevoNodo.next = this.head;
        }
    }

    public void imprimirLista() {
        if (this.head == null) {
            System.out.println("None");
            return;
        }

        Nodo<T> nodoActual = this.head;
        do {
            System.out.print(nodoActual.data + " -> ");
            nodoActual = nodoActual.next;
        } while (nodoActual != this.head);
        System.out.println("(vuelve a " + this.head.data + ")");
    }

    public boolean buscar(T dataBuscada) {
        if (this.head == null) {
            return false;
        }

        Nodo<T> nodoActual = this.head;
        do {
            if (nodoActual.data.equals(dataBuscada)) {
                return true;
            }
            nodoActual = nodoActual.next;
        } while (nodoActual != this.head);
        return false;
    }

    public void eliminar(T dataAEliminar) {
        if (this.head == null) {
            return;
        }

        if (this.head.data.equals(dataAEliminar)) {
            if (this.head.next == this.head) {
                this.head = null;
                return;
            }

            Nodo<T> nodoActual = this.head;
            while (nodoActual.next != this.head) {
                nodoActual = nodoActual.next;
            }

            nodoActual.next = this.head.next;
            this.head = this.head.next;
            return;
        }

        Nodo<T> nodoPrevio = this.head;
        Nodo<T> nodoActual = this.head.next;
        while (nodoActual != this.head) {
            if (nodoActual.data.equals(dataAEliminar)) {
                nodoPrevio.next = nodoActual.next;
                return;
            }
            nodoPrevio = nodoActual;
            nodoActual = nodoActual.next;
        }
    }

    public static void main(String[] args) {
        ListaCircularEnlazada<Integer> miLista = new ListaCircularEnlazada<>();
        miLista.insertarAlFinal(10);
        miLista.insertarAlFinal(20);
        miLista.insertarAlFinal(30);
        miLista.imprimirLista();

        miLista.insertarAlPrincipio(5);
        miLista.imprimirLista();

        System.out.println("¿Está el 20? " + miLista.buscar(20));
        System.out.println("¿Está el 99? " + miLista.buscar(99));

        miLista.eliminar(20);
        miLista.imprimirLista();

        miLista.eliminar(5);
        miLista.imprimirLista();

        miLista.eliminar(30);
        miLista.imprimirLista();
    }
}