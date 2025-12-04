using System;

public class PilaArreglo
{
    private int[] stack;
    private int maxSize;
    private int top;

    public PilaArreglo(int tamanoMax)
    {
        this.maxSize = tamanoMax;
        this.stack = new int[this.maxSize];
        this.top = -1;
    }

    public bool IsEmpty()
    {
        return this.top == -1;
    }

    public bool IsFull()
    {
        return this.top == this.maxSize - 1;
    }

    public void Push(int data)
    {
        if (this.IsFull())
        {
            Console.WriteLine("Error: Stack Overflow");
            return;
        }
        this.top++;
        this.stack[this.top] = data;
    }

    public int Pop()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine("Error: Stack Underflow");
            return -1;
        }
        int data = this.stack[this.top];
        this.top--;
        return data;
    }

    public int Peek()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine("Pila vacía");
            return -1;
        }
        return this.stack[this.top];
    }
}

public class pilaasrre
{
    public static void Main(string[] args)
    {
        PilaArreglo pila = new PilaArreglo(100);
        pila.Push(10);
        pila.Push(20);
        pila.Push(30);

        Console.WriteLine($"Elemento superior: {pila.Peek()}");
        Console.WriteLine($"Extrae elemento: {pila.Pop()}");
        Console.WriteLine($"Nuevo elemento superior: {pila.Peek()}");
    }
}