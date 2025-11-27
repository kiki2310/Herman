public class NodoDoble<T> {
    public T data;
    public NodoDoble<T> next;
    public NodoDoble<T> prev;

    public NodoDoble(T data) {
        this.data = data;
        this.next = null;
        this.prev = null;
    }
}

public class ListaDoblementeCircular<T> {
    private NodoDoble<T> head;

    public ListaDoblementeCircular() {
        this.head = null;
    }

    public void insertarAlPrincipio(T data) {
        NodoDoble<T> nuevoNodo = new NodoDoble<>(data);
        if (this.head == null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
            nuevoNodo.prev = this.head;
        } else {
            NodoDoble<T> tail = this.head.prev;
            nuevoNodo.next = this.head;
            this.head.prev = nuevoNodo;
            this.head = nuevoNodo;
            this.head.prev = tail;
            tail.next = this.head;
        }
    }

    public void insertarAlFinal(T data) {
        NodoDoble<T> nuevoNodo = new NodoDoble<>(data);
        if (this.head == null) {
            this.head = nuevoNodo;
            nuevoNodo.next = this.head;
            nuevoNodo.prev = this.head;
        } else {
            NodoDoble<T> tail = this.head.prev;
            tail.next = nuevoNodo;
            nuevoNodo.prev = tail;
            nuevoNodo.next = this.head;
            this.head.prev = nuevoNodo;
        }
    }

    public void imprimirLista() {
        if (this.head == null) {
            System.out.println("None");
            return;
        }

        NodoDoble<T> nodoActual = this.head;
        do {
            System.out.print(nodoActual.data + " <-> ");
            nodoActual = nodoActual.next;
        } while (nodoActual != this.head);
        System.out.println("(vuelve a " + this.head.data + ")");
    }

    public boolean buscar(T dataBuscada) {
        if (this.head == null) {
            return false;
        }

        NodoDoble<T> nodoActual = this.head;
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

        NodoDoble<T> nodoActual = this.head;
        boolean encontrado = false;
        do {
            if (nodoActual.data.equals(dataAEliminar)) {
                encontrado = true;
                break;
            }
            nodoActual = nodoActual.next;
        } while (nodoActual != this.head);

        if (!encontrado) return;

        if (nodoActual == this.head && this.head.next == this.head) {
            this.head = null;
            return;
        }

        if (nodoActual == this.head) {
            this.head = nodoActual.next;
        }

        nodoActual.prev.next = nodoActual.next;
        nodoActual.next.prev = nodoActual.prev;
    }

    public static void main(String[] args) {
        ListaDoblementeCircular<Integer> miLista = new ListaDoblementeCircular<>();
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