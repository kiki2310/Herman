class Mascota {
    constructor(nombre, tipo, edad, raza) {
        this.nombre = nombre;
        this.tipo = tipo;
        this.edad = edad;
        this.raza = raza;
    }
}
const cat = new Mascota("Firulais", "Perro", 3, "Labrador");
console.log(cat);