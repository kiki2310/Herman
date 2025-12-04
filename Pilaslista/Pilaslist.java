public class Nodo {
    public int data;
    public Nodo next;

    public Nodo(int data) {
        this.data = data;
        this.next = null;
    }
}

public class PilaLista {
    private Nodo top;

    public PilaLista() {
        this.top = null;
    }

    public boolean isEmpty() {
        return this.top == null;
    }

    public void push(int data) {
        Nodo nuevoNodo = new Nodo(data);
        nuevoNodo.next = this.top;
        this.top = nuevoNodo;
    }

    public int pop() {
        if (this.isEmpty()) {
            System.out.println("Error: Stack Underflow");
            return -1;
        }
        int data = this.top.data;
        this.top = this.top.next;
        return data;
    }

    public int peek() {
        if (this.isEmpty()) {
            System.out.println("Pila vacía");
            return -1;
        }
        return this.top.data;
    }

    public static void main(String[] args) {
        PilaLista pila = new PilaLista();
        pila.push(10);
        pila.push(20);
        pila.push(30);

        System.out.println("Elemento superior: " + pila.peek());
        System.out.println("Extrae elemento: " + pila.pop());
        System.out.println("Nuevo elemento superior: " + pila.peek());
    }
}