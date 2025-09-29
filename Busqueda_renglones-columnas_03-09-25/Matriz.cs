using System;
using System.Collections.Generic;

public class Matriz
{
    public static void Main(string[] args)
    {
        int[3][3] matriz = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
        
        int filas = matriz.Length;
        int columnas = matriz[0].Length;
        
        Console.WriteLine("--- Matriz Original ---");
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                Console.Write(matriz[i][j] + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\n--- columnas ---");
        for (int i = 0; i < columnas; i++) {
            for (int j = 0; j < filas; j++) {
                Console.Write(matriz[j][i] + " ");
            }
        }
        Console.WriteLine("\n--- filas ---");
        for (int i = 0; i < filas; i++) {
            for (int j = 0; j < columnas; j++) {
                Console.Write(matriz[i][j] + " ");
            }
        }
            Console.WriteLine(); 
        

        Console.WriteLine("\nfin del programa");
    }
}