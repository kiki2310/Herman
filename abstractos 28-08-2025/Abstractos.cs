using System;

public class Mascota
{
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public int Edad { get; set; }
    public string Raza { get; set; }
}
public class Program
{
    public static void Main(string[] args)
    {
         Mascota miMascota = new Mascota 
        {
            Nombre = "Fido",
            Tipo = "Perro",
            Edad = 5,
            Raza = "Labrador"
        };
        Console.WriteLine($"Nombre: {miMascota.Nombre}, Tipo: {miMascota.Tipo}, Edad: {miMascota.Edad}, Raza: {miMascota.Raza}");
    }
}