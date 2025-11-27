using System;
using System.Collections.Generic;

public class NodoDoble<T>
{
    public T Data { get; set; }
    public NodoDoble<T> Next { get; set; }
    public NodoDoble<T> Prev { get; set; }

    public NodoDoble(T data)
    {
        this.Data = data;
        this.Next = null;
        this.Prev = null;
    }
}

public class ListaDoblementeEnlazada<T>
{
    private NodoDoble<T> head;
    private NodoDoble<T> tail;

    public ListaDoblementeEnlazada()
    {
        this.head = null;
        this.tail = null;
    }

    public void InsertarAlPrincipio(T data)
    {
        NodoDoble<T> nuevoNodo = new NodoDoble<T>(data);
        if (this.head == null)
        {
            this.head = nuevoNodo;
            this.tail = nuevoNodo;
        }
        else
        {
            nuevoNodo.Next = this.head;
            this.head.Prev = nuevoNodo;
            this.head = nuevoNodo;
        }
    }

    public void InsertarAlFinal(T data)
    {
        NodoDoble<T> nuevoNodo = new NodoDoble<T>(data);
        if (this.head == null)
        {
            this.head = nuevoNodo;
            this.tail = nuevoNodo;
        }
        else
        {
            this.tail.Next = nuevoNodo;
            nuevoNodo.Prev = this.tail;
            this.tail = nuevoNodo;
        }
    }

    public void ImprimirLista()
    {
        NodoDoble<T> nodoActual = this.head;
        while (nodoActual != null)
        {
            Console.Write($"{nodoActual.Data} <-> ");
            nodoActual = nodoActual.Next;
        }
        Console.WriteLine("None");
    }

    public bool Buscar(T dataBuscada)
    {
        NodoDoble<T> nodoActual = this.head;
        while (nodoActual != null)
        {
            if (EqualityComparer<T>.Default.Equals(nodoActual.Data, dataBuscada))
            {
                return true;
            }
            nodoActual = nodoActual.Next;
        }
        return false;
    }

    public void Eliminar(T dataAEliminar)
    {
        NodoDoble<T> nodoActual = this.head;
        while (nodoActual != null)
        {
            if (EqualityComparer<T>.Default.Equals(nodoActual.Data, dataAEliminar))
            {
                break;
            }
            nodoActual = nodoActual.Next;
        }

        if (nodoActual == null)
        {
            return;
        }

        if (nodoActual.Prev != null)
        {
            nodoActual.Prev.Next = nodoActual.Next;
        }
        else
        {
            this.head = nodoActual.Next;
        }

        if (nodoActual.Next != null)
        {
            nodoActual.Next.Prev = nodoActual.Prev;
        }
        else
        {
            this.tail = nodoActual.Prev;
        }
    }
}

public class ProgramDoble
{
    public static void Main(string[] args)
    {
        ListaDoblementeEnlazada<int> miLista = new ListaDoblementeEnlazada<int>();
        miLista.InsertarAlFinal(10);
        miLista.InsertarAlFinal(20);
        miLista.InsertarAlFinal(30);
        miLista.ImprimirLista();

        miLista.InsertarAlPrincipio(5);
        miLista.ImprimirLista();

        Console.WriteLine($"¿Está el 20? {miLista.Buscar(20)}");
        Console.WriteLine($"¿Está el 99? {miLista.Buscar(99)}");

        miLista.Eliminar(20);
        miLista.ImprimirLista();

        miLista.Eliminar(5);
        miLista.ImprimirLista();

        miLista.Eliminar(30);
        miLista.ImprimirLista();
    }
}