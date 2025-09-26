import java.util.Scanner;

public class Miniexamen {
    public static void main(String[] args) {
       Scanner scanner = new Scanner(System.in);
         int[] arreglo = new int[10];
         int suma=0;
         int max=0;
         int min=1;
            for (int i = 0; i < 10; i++) {
                System.out.print("Ingrese el dato " + (i+1)+ ": ");
                arreglo[i] = scanner.nextInt();
                suma += arreglo[i];
                
                if (arreglo[i] > max) {
                    max = arreglo[i];                
                }
                if (min > arreglo[i]) {
                    min=arreglo[i];
                }

                
            }

            System.out.println("Arreglo ingresado:");
            for (int num : arreglo) {
                System.out.print(num + " ");

            }
            System.out.println();
            scanner.close();
            float Promedio=0;
            Promedio=suma/10;
            System.out.println("Suma: " + suma);
            System.out.println("Promedio: " +Promedio);
            System.out.println("Valor mínimo: " + min);
            System.out.println("Valor máximo: " + max);

    }

}
