
// En Java, los arreglos son objetos que contienen un número fijo de valores de un solo tipo.
// La longitud de un arreglo se establece cuando se crea y no puede cambiar.
// Para colecciones dinámicas, se suele usar ArrayList.

import java.util.Arrays;

public class Arreglos {
    public static void main(String[] args) {
        // 1. Declaración e inicialización
        // Declarar un arreglo de enteros
        int[] numeros;

        // Inicializar el arreglo con un tamaño de 5 (los valores por defecto son 0)
        numeros = new int[5];

        // Asignamos valores a nuestro arreglo de números
        numeros[0] = 100;
        numeros[1] = 200;
        System.out.println("Arreglo de números: " + Arrays.toString(numeros));

        // Declarar e inicializar con valores en una sola línea
        String[] frutas = {"Manzana", "Banana", "Naranja"};
        System.out.println("Arreglo inicial de frutas: " + Arrays.toString(frutas));

        // 2. Acceder a elementos
        // Los índices comienzan en 0
        String primeraFruta = frutas[0]; // Accede a "Manzana"
        System.out.println("La primera fruta es: " + primeraFruta);

        // 3. Modificar elementos
        frutas[1] = "Uva"; // Cambia "Banana" por "Uva"
        System.out.println("Arreglo modificado: " + Arrays.toString(frutas));

        // 4. Longitud del arreglo
        // .length es una propiedad, no un método
        System.out.println("La longitud del arreglo es: " + frutas.length);

        // 5. Iterar sobre un arreglo (usando un bucle "for-each")
        System.out.println("Iterando sobre el arreglo:");
        for (String fruta : frutas) {
            System.out.println("- " + fruta);
        }
    }
}
