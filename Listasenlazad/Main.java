class Nodo<T> {
    T data;
    Nodo<T> next;

    public Nodo(T data) {
        this.data = data;
        this.next = null;
    }
}

class ListaEnlazada<T> {
    private Nodo<T> head;

    public ListaEnlazada() {
        this.head = null;
    }

    public void insertarAlPrincipio(T data) {
        Nodo<T> nuevoNodo = new Nodo<>(data);
        nuevoNodo.next = this.head;
        this.head = nuevoNodo;
    }

    public void insertarAlFinal(T data) {
        Nodo<T> nuevoNodo = new Nodo<>(data);
        if (this.head == null) {
            this.head = nuevoNodo;
            return;
        }

        Nodo<T> ultimoNodo = this.head;
        while (ultimoNodo.next != null) {
            ultimoNodo = ultimoNodo.next;
        }
        
        ultimoNodo.next = nuevoNodo;
    }

    public void imprimirLista() {
        Nodo<T> nodoActual = this.head;
        while (nodoActual != null) {
            System.out.print(nodoActual.data + " -> ");
            nodoActual = nodoActual.next;
        }
        System.out.println("None");
    }

    public boolean buscar(T dataBuscada) {
        Nodo<T> nodoActual = this.head;
        while (nodoActual != null) {
            if (nodoActual.data.equals(dataBuscada)) {
                return true;
            }
            nodoActual = nodoActual.next;
        }
        return false;
    }

    // Eliminar un elemento de la lista
    public void eliminar(T dataAEliminar) {
        Nodo<T> nodoActual = this.head;
        Nodo<T> nodoPrevio = null;


        if (nodoActual != null && nodoActual.data.equals(dataAEliminar)) {
            this.head = nodoActual.next;
            return;
        }

        while (nodoActual != null && !nodoActual.data.equals(dataAEliminar)) {
            nodoPrevio = nodoActual;
            nodoActual = nodoActual.next;
        }

        if (nodoActual == null) {
            return;
        }


        nodoPrevio.next = nodoActual.next;
    }
}

public class Main {
    public static void main(String[] args) {
        ListaEnlazada<Integer> miLista = new ListaEnlazada<>();
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
    }
}
