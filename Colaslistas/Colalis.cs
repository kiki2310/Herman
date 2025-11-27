using System;

public class ColaLista
{
    private Nodo? front;
    private Nodo? rear;

    public ColaLista()
    {
        this.front = null;
        this.rear = null;
    }

    public bool IsEmpty()
    {
        return this.front == null;
    }

    public void Enqueue(int data)
    {
        Nodo nuevoNodo = new Nodo(data);
        if (this.rear == null)
        {
            this.front = nuevoNodo;
            this.rear = nuevoNodo;
            return;
        }
        this.rear.Next = nuevoNodo;
        this.rear = nuevoNodo;
    }

    public int Dequeue()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine("Error: Cola subdesbordada (Underflow)");
            return -1;
        }

        int data = this.front.Data;
        this.front = this.front.Next;

        if (this.front == null)
        {
            this.rear = null;
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
        return this.front.Data;
    }
}

public class ColaListap
{
    public static void Main(string[] args)
    {
        ColaLista cola = new ColaLista();
        cola.Enqueue(10);
        cola.Enqueue(20);
        cola.Enqueue(30);

        Console.WriteLine($"Elemento frontal: {cola.Peek()}");
        Console.WriteLine($"Elimina elemento: {cola.Dequeue()}");
        Console.WriteLine($"Nuevo elemento frontal: {cola.Peek()}");

        cola.Enqueue(40);
        Console.WriteLine($"Elimina elemento: {cola.Dequeue()}");
        Console.WriteLine($"Elimina elemento: {cola.Dequeue()}");
        Console.WriteLine($"Elimina elemento: {cola.Dequeue()}");
        Console.WriteLine($"Elimina elemento: {cola.Dequeue()}");
    }
}