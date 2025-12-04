public class PilaArreglo {
    private int[] stack;
    private int maxSize;
    private int top;

    public PilaArreglo(int tamanoMax) {
        this.maxSize = tamanoMax;
        this.stack = new int[this.maxSize];
        this.top = -1;
    }

    public boolean isEmpty() {
        return this.top == -1;
    }

    public boolean isFull() {
        return this.top == this.maxSize - 1;
    }

    public void push(int data) {
        if (this.isFull()) {
            System.out.println("Error: Stack Overflow");
            return;
        }
        this.top++;
        this.stack[this.top] = data;
    }

    public int pop() {
        if (this.isEmpty()) {
            System.out.println("Error: Stack Underflow");
            return -1;
        }
        int data = this.stack[this.top];
        this.top--;
        return data;
    }

    public int peek() {
        if (this.isEmpty()) {
            System.out.println("Pila vacía");
            return -1;
        }
        return this.stack[this.top];
    }

    public static void main(String[] args) {
        PilaArreglo pila = new PilaArreglo(100);
        pila.push(10);
        pila.push(20);
        pila.push(30);

        System.out.println("Elemento superior: " + pila.peek());
        System.out.println("Extrae elemento: " + pila.pop());
        System.out.println("Nuevo elemento superior: " + pila.peek());
    }
}