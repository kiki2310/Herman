using System;
using System.Collections.Generic;

public class Nodo<T>
{
    public T Data { get; set; }
    public Nodo<T> Next { get; set; }

    public Nodo(T data)
    {
        this.Data = data;
        this.Next = null;
    }
}

public class ListaCircularEnlazada<T>
{
    private Nodo<T> head;

    public ListaCircularEnlazada()
    {
        this.head = null;
    }

    public void InsertarAlPrincipio(T data)
    {
        Nodo<T> nuevoNodo = new Nodo<T>(data);
        if (this.head == null)
        {
            this.head = nuevoNodo;
            nuevoNodo.Next = this.head;
        }
        else
        {
            // Encontrar el último nodo para actualizar su puntero
            Nodo<T> tail = this.head;
            while (tail.Next != this.head)
            {
                tail = tail.Next;
            }
            nuevoNodo.Next = this.head;
            this.head = nuevoNodo;
            tail.Next = this.head; // El último nodo ahora apunta al nuevo head
        }
    }

    public void InsertarAlFinal(T data)
    {
        Nodo<T> nuevoNodo = new Nodo<T>(data);
        if (this.head == null)
        {
            this.head = nuevoNodo;
            nuevoNodo.Next = this.head;
        }
        else
        {
            Nodo<T> nodoActual = this.head;
            while (nodoActual.Next != this.head)
            {
                nodoActual = nodoActual.Next;
            }
            nodoActual.Next = nuevoNodo;
            nuevoNodo.Next = this.head;
        }
    }

    public void ImprimirLista()
    {
        if (this.head == null)
        {
            Console.WriteLine("None");
            return;
        }

        Nodo<T> nodoActual = this.head;
        do
        {
            Console.Write($"{nodoActual.Data} -> ");
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

        Nodo<T> nodoActual = this.head;
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

        // Encontrar el nodo a eliminar y su predecesor
        Nodo<T> nodoPrevio = null;
        Nodo<T> nodoActual = this.head;

        do
        {
            if (EqualityComparer<T>.Default.Equals(nodoActual.Data, dataAEliminar))
            {
                break; // Nodo encontrado
            }
            nodoPrevio = nodoActual;
            nodoActual = nodoActual.Next;
        } while (nodoActual != this.head);

        // Si el nodo no se encontró (volvimos al inicio)
        if (!EqualityComparer<T>.Default.Equals(nodoActual.Data, dataAEliminar))
        {
            return; // El elemento no está en la lista
        }

        // Caso 1: La lista tiene un solo nodo
        if (nodoActual.Next == nodoActual)
        {
            this.head = null;
        }
        // Caso 2: El nodo a eliminar es el head
        else if (nodoActual == this.head)
        {
            nodoPrevio.Next = this.head.Next; // El último nodo apunta al nuevo head
            this.head = this.head.Next;
        }
        // Caso 3: El nodo está en medio o al final
        else
        {
            nodoPrevio.Next = nodoActual.Next;
        }
    }
}

public class Lisciren
{
    public static void Main(string[] args)
    {
        ListaCircularEnlazada<int> miLista = new ListaCircularEnlazada<int>();
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