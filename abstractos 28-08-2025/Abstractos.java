// Para poder crear un objeto ('new Mascota(...)'), la clase no puede ser 'abstract'.
// Esta es la solución más simple para que tu código se ejecute.
class Mascota {
    public String nombre;
    public String tipo;
    public int edad;
    public String raza;

    public Mascota(String nombre, String tipo, int edad, String raza) {
        this.nombre = nombre;
        this.tipo = tipo;
        this.edad = edad;
        this.raza = raza;
    }

}



public class Abstractos {
    public static void main(String[] args) {
        // Ahora que 'Mascota' es una clase normal (concreta), podemos crearla directamente.
        // La sintaxis rara '{};' ya no es necesaria.
        Mascota ca = new Mascota("Miau", "Gato", 2, "Siames");  
    System.out.println("Nombre: " + ca.nombre);
    System.out.println("Tipo: " + ca.tipo);
    System.out.println("Edad: " + ca.edad + " años");
    System.out.println("Raza: " + ca.raza);
    }
}
