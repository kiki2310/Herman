public class ColaArreglo {
    private int[] queue;
    private int maxSize;
    private int front;
    private int rear;

    public ColaArreglo(int tamanoMax) {
        this.maxSize = tamanoMax;
        this.queue = new int[this.maxSize];
        this.front = -1;
        this.rear = -1;
    }

    public boolean isFull() {
        return (this.front == 0 && this.rear == this.maxSize - 1) || (this.front == this.rear + 1);
    }

    public boolean isEmpty() {
        return this.front == -1;
    }

    public void enqueue(int data) {
        if (this.isFull()) {
            System.out.println("Error: Cola desbordada (Overflow)");
            return;
        }

        if (this.front == -1) {
            this.front = 0;
        }

        this.rear = (this.rear + 1) % this.maxSize;
        this.queue[this.rear] = data;
    }

    public int dequeue() {
        if (this.isEmpty()) {
            System.out.println("Error: Cola subdesbordada (Underflow)");
            return -1;
        }

        int data = this.queue[this.front];
        if (this.front == this.rear) {
            this.front = -1;
            this.rear = -1;
        } else {
            this.front = (this.front + 1) % this.maxSize;
        }
        return data;
    }

    public int peek() {
        if (this.isEmpty()) {
            System.out.println("Cola vacía");
            return -1;
        }
        return this.queue[this.front];
    }

    public static void main(String[] args) {
        ColaArreglo cola = new ColaArreglo(5);
        cola.enqueue(10);
        cola.enqueue(20);
        cola.enqueue(30);

        System.out.println("Elemento frontal: " + cola.peek());
        System.out.println("Elimina elemento: " + cola.dequeue());
        System.out.println("Nuevo elemento frontal: " + cola.peek());

        cola.enqueue(40);
        cola.enqueue(50);
        cola.enqueue(60);
        System.out.println("Elimina elemento: " + cola.dequeue());
    }
}