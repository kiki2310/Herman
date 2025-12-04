using System;

public class ColaArreglo
{
    private int[] queue;
    private int maxSize;
    private int front;
    private int rear;

    public ColaArreglo(int tamanoMax)
    {
        this.maxSize = tamanoMax;
        this.queue = new int[this.maxSize];
        this.front = -1;
        this.rear = -1;
    }

    public bool IsFull()
    {
        return (this.front == 0 && this.rear == this.maxSize - 1) || (this.front == this.rear + 1);
    }

    public bool IsEmpty()
    {
        return this.front == -1;
    }

    public void Enqueue(int data)
    {
        if (this.IsFull())
        {
            Console.WriteLine("Error: Cola desbordada (Overflow)");
            return;
        }

        if (this.front == -1)
        {
            this.front = 0;
        }

        this.rear = (this.rear + 1) % this.maxSize;
        this.queue[this.rear] = data;
    }

    public int Dequeue()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine("Error: Cola subdesbordada (Underflow)");
            return -1;
        }

        int data = this.queue[this.front];
        if (this.front == this.rear)
        {
            this.front = -1;
            this.rear = -1;
        }
        else
        {
            this.front = (this.front + 1) % this.maxSize;
        }
        return data;
    }

    public int Peek()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine("Cola vacía");
            return -1;
        }
        return this.queue[this.front];
    }
}

public class colasaarepro
{
    public static void Main(string[] args)
    {
        ColaArreglo cola = new ColaArreglo(5);
        cola.Enqueue(10);
        cola.Enqueue(20);
        cola.Enqueue(30);

        Console.WriteLine($"Elemento frontal: {cola.Peek()}");
        Console.WriteLine($"Elimina elemento: {cola.Dequeue()}");
        Console.WriteLine($"Nuevo elemento frontal: {cola.Peek()}");

        cola.Enqueue(40);
        cola.Enqueue(50);
        cola.Enqueue(60);
        Console.WriteLine($"Elimina elemento: {cola.Dequeue()}");
    }
}