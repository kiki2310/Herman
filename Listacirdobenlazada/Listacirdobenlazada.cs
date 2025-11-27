using System;
using System.Collections.Generic;

namespace ListaCircularDoble
{

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

public class ListaDoblementeCircular<T>
{
    private NodoDoble<T> head;

    public ListaDoblementeCircular()
    {
        this.head = null;
    }

    public void InsertarAlPrincipio(T data)
    {
        NodoDoble<T> nuevoNodo = new NodoDoble<T>(data);
        if (this.head == null)
        {
            this.head = nuevoNodo;
            nuevoNodo.Next = this.head;
            nuevoNodo.Prev = this.head;
        }
        else
        {
            NodoDoble<T> tail = this.head.Prev;
            nuevoNodo.Next = this.head;
            this.head.Prev = nuevoNodo;
            this.head = nuevoNodo;
            this.head.Prev = tail;
            tail.Next = this.head;
        }
    }

    public void InsertarAlFinal(T data)
    {
        NodoDoble<T> nuevoNodo = new NodoDoble<T>(data);
        if (this.head == null)
        {
            this.head = nuevoNodo;
            nuevoNodo.Next = this.head;
            nuevoNodo.Prev = this.head;
        }
        else
        {
            NodoDoble<T> tail = this.head.Prev;
            tail.Next = nuevoNodo;
            nuevoNodo.Prev = tail;
            nuevoNodo.Next = this.head;
            this.head.Prev = nuevoNodo;
        }
    }

    public void ImprimirLista()
    {
        if (this.head == null)
        {
            Console.WriteLine("None");
            return;
        }

        NodoDoble<T> nodoActual = this.head;
        do
        {
            Console.Write($"{nodoActual.Data} <-> ");
            nodoActual = nodoActual.Next;
        } while (nodoActual != this.head);
        Console.WriteLine($"(vuelve a {this.head.Data})");
    }

    public bool Buscar(T dataBuscada)
    {
        if (this.head == null)
        {
            return false;
        }

        NodoDoble<T> nodoActual = this.head;
        do
        {
            if (EqualityComparer<T>.Default.Equals(nodoActual.Data, dataBuscada))
            {
                return true;
            }
            nodoActual = nodoActual.Next;
        } while (nodoActual != this.head);
        return false;
    }

    public void Eliminar(T dataAEliminar)
    {
        if (this.head == null)
        {
            return;
        }

        // 1. Encontrar el nodo a eliminar
        NodoDoble<T> nodoActual = this.head;
        do
        {
            if (EqualityComparer<T>.Default.Equals(nodoActual.Data, dataAEliminar))
            {
                break; // Nodo encontrado
            }
            nodoActual = nodoActual.Next;
        } while (nodoActual != this.head);

        // Si después del bucle no encontramos el dato, no está en la lista
        if (!EqualityComparer<T>.Default.Equals(nodoActual.Data, dataAEliminar))
        {
            return;
        }

        // 2. Re-enlazar los nodos según el caso

        // Caso A: La lista tiene un solo nodo
        if (nodoActual == this.head && this.head.Next == this.head)
        {
            this.head = null;
        }
        // Caso B: El nodo a eliminar es el head (y hay más de un nodo)
        else if (nodoActual == this.head)
        {
            this.head = nodoActual.Next;
        }

        // Caso General (incluye B y C: nodo en medio o al final)
        // Se actualizan los punteros del nodo anterior y del siguiente para "saltarse" el nodoActual.
        nodoActual.Prev.Next = nodoActual.Next;
        nodoActual.Next.Prev = nodoActual.Prev;
    }
}

public class Listacirdobenlazadaprogram
{
    public static void Main(string[] args)
    {
        ListaDoblementeCircular<int> miLista = new ListaDoblementeCircular<int>();
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
}