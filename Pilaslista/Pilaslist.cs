using System;

public class Nodo
{
    public int Data { get; set; }
    public Nodo? Next { get; set; }

    public Nodo(int data)
    {
        this.Data = data;
        this.Next = null;
    }
}

public class PilaLista
{
    private Nodo? top;

    public PilaLista()
    {
        this.top = null;
    }

    public bool IsEmpty()
    {
        return this.top == null;
    }

    public void Push(int data)
    {
        Nodo nuevoNodo = new Nodo(data);
        nuevoNodo.Next = this.top;
        this.top = nuevoNodo;
    }

    public int Pop()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine("Error: Stack Underflow");
            return -1;
        }
        int data = this.top.Data;
        this.top = this.top.Next;
        return data;
    }

    public int Peek()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine("Pila vacía");
            return -1;
        }
        return this.top.Data;
    }
}

public class Pilaslista
{
    public static void Main(string[] args)
    {
        PilaLista pila = new PilaLista();
        pila.Push(10);
        pila.Push(20);
        pila.Push(30);

        Console.WriteLine($"Elemento superior: {pila.Peek()}");
        Console.WriteLine($"Extrae elemento: {pila.Pop()}");
        Console.WriteLine($"Nuevo elemento superior: {pila.Peek()}");
    }
}