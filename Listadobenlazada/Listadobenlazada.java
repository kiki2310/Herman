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

public class ListaDoblementeEnlazada<T> {
    private NodoDoble<T> head;
    private NodoDoble<T> tail;

    public ListaDoblementeEnlazada() {
        this.head = null;
        this.tail = null;
    }

    public void insertarAlPrincipio(T data) {
        NodoDoble<T> nuevoNodo = new NodoDoble<>(data);
        if (this.head == null) {
            this.head = nuevoNodo;
            this.tail = nuevoNodo;
        } else {
            nuevoNodo.next = this.head;
            this.head.prev = nuevoNodo;
            this.head = nuevoNodo;
        }
    }

    public void insertarAlFinal(T data) {
        NodoDoble<T> nuevoNodo = new NodoDoble<>(data);
        if (this.head == null) {
            this.head = nuevoNodo;
            this.tail = nuevoNodo;
        } else {
            this.tail.next = nuevoNodo;
            nuevoNodo.prev = this.tail;
            this.tail = nuevoNodo;
        }
    }

    public void imprimirLista() {
        NodoDoble<T> nodoActual = this.head;
        while (nodoActual != null) {
            System.out.print(nodoActual.data + " <-> ");
            nodoActual = nodoActual.next;
        }
        System.out.println("None");
    }

    public boolean buscar(T dataBuscada) {
        NodoDoble<T> nodoActual = this.head;
        while (nodoActual != null) {
            if (nodoActual.data.equals(dataBuscada)) {
                return true;
            }
            nodoActual = nodoActual.next;
        }
        return false;
    }

    public void eliminar(T dataAEliminar) {
        NodoDoble<T> nodoActual = this.head;
        while (nodoActual != null) {
            if (nodoActual.data.equals(dataAEliminar)) {
                break;
            }
            nodoActual = nodoActual.next;
        }

        if (nodoActual == null) {
            return;
        }

        if (nodoActual.prev != null) {
            nodoActual.prev.next = nodoActual.next;
        } else {
            this.head = nodoActual.next;
        }

        if (nodoActual.next != null) {
            nodoActual.next.prev = nodoActual.prev;
        } else {
            this.tail = nodoActual.prev;
        }
    }

    public static void main(String[] args) {
        ListaDoblementeEnlazada<Integer> miLista = new ListaDoblementeEnlazada<>();
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