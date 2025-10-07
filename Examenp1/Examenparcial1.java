import java.util.Scanner;

public class Examenparcial1 {

    public static void main(String[] args) {
        String pos[] = new String[27];
        int array[] = {0,1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15};
        int contpar = 0;
        int continpar = 0;                
        System.out.println("--- Conteo de pares y impares ---");

        for(int i=0;i< array.length;i++){
            if (array[i]%2==0) {
                contpar++;
            }else{
                continpar++;
            }
        }
        System.out.println("---Arreglo original----");
        for (int num : array) {
            System.out.print(num + " ");
        }
        System.out.println("\nNumeros pares: "+contpar);
        System.out.println("Numeros impares: "+continpar);

        int matriz[][]= {
                {1,2,3},
                {4,5,6},
                {7,8,9} 
        };
        int sumcolum=0;
         System.out.println("--- Suma columnas ---");
        for (int i = 0; i < matriz.length; i++) {
            for (int j = 0; j < matriz[0].length; j++) {
                System.out.print(matriz[i][j] + " ");
            }
            System.out.println();
        }
        for (int i = 0; i < matriz.length; i++) {
            for (int j = 0; j < matriz[0].length; j++) {
                sumcolum += matriz[j][i];
            }
            System.out.println("Suma de la columna "+(i+1)+": "+sumcolum);
            sumcolum=0;
        }
        int matriz3d[][][] =new int[3][3][3];
        for (int i=0;i <3;i++){
            for(int j=0;j<3;j++){
                for(int k=0;k<3;k++){
                    matriz3d[i][j][k]= (int)(Math.random()*11);
                    System.out.print(matriz3d[i][j][k]+" ");
                }
                System.out.println();
            }
            System.out.println();
        }
        System.out.println("Ingresa un numero entero que quieras buscar: ");
        Scanner scanner = new Scanner(System.in);
        while (!scanner.hasNextInt()) {
            System.out.println("Error: Eso no es un número entero. Por favor, intente de nuevo.");
            scanner.next(); 
            System.out.print("Ingrese un número entero: ");
        }
        int numbuscado = scanner.nextInt();
        scanner.close();
        int crep=0;
        boolean encontrado=false;
        System.out.println("---Busqueda en Matriz 3D ---");
        for (int i=0;i <3;i++){
            for(int j=0;j<3;j++){
                for(int k=0;k<3;k++){
                if (matriz3d[i][j][k]==numbuscado) {
                        encontrado=true;
                        pos[crep]="Posicion: ["+i+"]["+j+"]["+k+"]";
                        crep++;
                    }
                }
            }
        }
        if (crep!=0) {
        System.out.println("Se repiten en estas posiciones");
            for(int i=0;i<crep;i++){
            System.out.println(pos[i]);                
        }    
        }else{
                    System.out.println("No se repite en ninguna posicion");

        }
        
        
    }

}
