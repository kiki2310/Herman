
using System;

public class Program
{
    public static void Main(string[] args)
    {
        // 1. Declaración e inicialización
        // Declarar e inicializar un arreglo de enteros con valores.
        int[] numeros = { 10, 20, 30, 40, 50 };
        Console.WriteLine($"Arreglo inicial: [{string.Join(", ", numeros)}]");

        // 2. Acceder a elementos
        // Los índices comienzan en 0.
        int primerNumero = numeros[0]; // Accede al 10
        Console.WriteLine($"El primer número es: {primerNumero}");

        // 3. Modificar elementos
        numeros[2] = 35; // Cambia el tercer elemento (30) a 35.
        Console.WriteLine($"Arreglo modificado: [{string.Join(", ", numeros)}]");

        // 4. Longitud del arreglo
        // La propiedad Length contiene el número de elementos.
        Console.WriteLine($"La longitud del arreglo es: {numeros.Length}");

        // 5. Iterar sobre un arreglo (usando un bucle foreach)
        Console.WriteLine("Iterando sobre el arreglo:");
        foreach (int numero in numeros)
        {
            Console.WriteLine($"- {numero}");
        }

    }
}
