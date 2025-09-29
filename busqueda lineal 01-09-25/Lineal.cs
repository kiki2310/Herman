using System;
using System.Collections.Generic; // Necesario para usar List<T>

public class Lineal
{
    public static int BusquedaLineal(List<int> arreglo, int objetivo)
    {
        for (int i = 0; i < arreglo.Count; i++)
        {
            if (arreglo[i] == objetivo)
            {
                return i; // Retorna el índice si se encuentra el elemento.
            }
        }
        return -1; // Retorna -1 si no se encuentra el elemento.
    }

    // Ya no se necesita 'ref' porque las listas son tipos de referencia y su tamaño puede cambiar.
    public static void InsertarNumero(List<int> arreglo, int nuevoValor, int posicion, int T)
    {
        for (int i = arreglo.Count; i > posicion; i--)
        {
            if (i < T) // Asegura que no se exceda el tamaño máximo T
            {
                if (i == arreglo.Count)
                {
                    arreglo.Add(arreglo[i - 1]); // Añade un nuevo elemento al final si es necesario
                }
                else
                {
                    arreglo[i] = arreglo[i - 1]; // Desplaza los elementos hacia la derecha
                }
            }
        }
        arreglo[posicion] = nuevoValor; // Inserta el nuevo valor en la posición
    }
    public static void ImprimirArreglo(List<int> arreglo)
    {
        Console.Write("Arreglo: ");
        Console.WriteLine(string.Join(" ", arreglo));

    }
    public static void Main(string[] args)
    {

        int T=5;
        List<int> arreglo = new List<int> { 10, 20, 30, 40, 50 };
        ImprimirArreglo(arreglo);
        Console.Write("Introduce el elemento a buscar: ");
        if (!int.TryParse(Console.ReadLine(), out int objetivo))
        {
            Console.WriteLine("Error: Debes introducir un número entero válido.");
            return;
        }
        int indice = BusquedaLineal(arreglo, objetivo);
        if (indice != -1)
        {
            Console.WriteLine("Elemento encontrado en el índice: " + indice);
        }
        else
        {
            Console.WriteLine("Elemento no encontrado.");
        }
        Console.WriteLine("Valor para insertar:");
        if (!int.TryParse(Console.ReadLine(), out int nuevoValor))
        {
            Console.WriteLine("Error: Debes introducir un número entero válido.");
            return;
        }
        Console.WriteLine("Posición para insertar:");
        if (!int.TryParse(Console.ReadLine(), out int posicion))
        {
            Console.WriteLine("Error: Debes introducir un número entero válido.");
            return;
        }
        InsertarNumero(arreglo, nuevoValor, posicion, T);
        ImprimirArreglo(arreglo);
        Console.WriteLine("fin del programa");
    }
}
