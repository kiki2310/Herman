using System;
using System.Collections.Generic; 

namespace EstructurasDeDatos
{
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

    public class ListaEnlazada<T>
    {
        private Nodo<T> head;

        public ListaEnlazada()
        {
            this.head = null;
        }

        public void InsertarAlPrincipio(T data)
        {
            Nodo<T> nuevoNodo = new Nodo<T>(data);
            nuevoNodo.Next = this.head;
            this.head = nuevoNodo;
        }

        public void InsertarAlFinal(T data)
        {
            Nodo<T> nuevoNodo = new Nodo<T>(data);
            if (this.head == null)
            {
                this.head = nuevoNodo;
                return;
            }

            Nodo<T> ultimoNodo = this.head;
            while (ultimoNodo.Next != null)
            {
                ultimoNodo = ultimoNodo.Next;
            }

            ultimoNodo.Next = nuevoNodo;
        }

        public void ImprimirLista()
        {
            Nodo<T> nodoActual = this.head;
            while (nodoActual != null)
            {
                Console.Write($"{nodoActual.Data} -> ");
                nodoActual = nodoActual.Next;
            }
            Console.WriteLine("null");
        }

        public bool Buscar(T dataBuscada)
        {
            Nodo<T> nodoActual = this.head;
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
            Nodo<T> nodoActual = this.head;
            Nodo<T> nodoPrevio = null;

            if (nodoActual == null) return;

            if (EqualityComparer<T>.Default.Equals(nodoActual.Data, dataAEliminar))
            {
                this.head = nodoActual.Next;
                return;
            }

            while (nodoActual != null && !EqualityComparer<T>.Default.Equals(nodoActual.Data, dataAEliminar))
            {
                nodoPrevio = nodoActual;
                nodoActual = nodoActual.Next;
            }

            if (nodoActual == null)
            {
                return;
            }

            nodoPrevio.Next = nodoActual.Next;
        }
    }

    public class Programlista
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Probando Lista Enlazada ---");
            
            ListaEnlazada<int> miLista = new ListaEnlazada<int>();
            
            miLista.InsertarAlFinal(10);
            miLista.InsertarAlFinal(20);
            miLista.InsertarAlFinal(30);
            miLista.ImprimirLista(); 


            miLista.InsertarAlPrincipio(5);
            miLista.ImprimirLista(); 

            Console.WriteLine($"¿Está el 20? {miLista.Buscar(20)}"); 
            Console.WriteLine($"¿Está el 99? {miLista.Buscar(99)}");

            Console.WriteLine("Eliminando el 20...");
            miLista.Eliminar(20);
            miLista.ImprimirLista(); 
            
            Console.WriteLine("Eliminando el 5...");
            miLista.Eliminar(5);
            miLista.ImprimirLista(); 
            
            Console.WriteLine("--- Fin del programa ---");
        }
    }
}