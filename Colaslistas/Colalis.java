public class Nodo {
    public int data;
    public Nodo next;

    public Nodo(int data) {
        this.data = data;
        this.next = null;
    }
}

public class ColaLista {
    private Nodo front;
    private Nodo rear;

    public ColaLista() {
        this.front = null;
        this.rear = null;
    }

    public boolean isEmpty() {
        return this.front == null;
    }

    public void enqueue(int data) {
        Nodo nuevoNodo = new Nodo(data);
        if (this.rear == null) {
            this.front = nuevoNodo;
            this.rear = nuevoNodo;
            return;
        }
        this.rear.next = nuevoNodo;
        this.rear = nuevoNodo;
    }

    public int dequeue() {
        if (this.isEmpty()) {
            System.out.println("Error: Cola subdesbordada (Underflow)");
            return -1;
        }

        int data = this.front.data;
        this.front = this.front.next;

        if (this.front == null) {
            this.rear = null;
        }
        return data;
    }

    public int peek() {
        if (this.isEmpty()) {
            System.out.println("Cola vacía");
            return -1;
        }
        return this.front.data;
    }

    public static void main(String[] args) {
        ColaLista cola = new ColaLista();
        cola.enqueue(10);
        cola.enqueue(20);
        cola.enqueue(30);

        System.out.println("Elemento frontal: " + cola.peek());
        System.out.println("Elimina elemento: " + cola.dequeue());
        System.out.println("Nuevo elemento frontal: " + cola.peek());

        cola.enqueue(40);
        System.out.println("Elimina elemento: " + cola.dequeue());
        System.out.println("Elimina elemento: " + cola.dequeue());
        System.out.println("Elimina elemento: " + cola.dequeue());
        System.out.println("Elimina elemento: " + cola.dequeue());
    }
}