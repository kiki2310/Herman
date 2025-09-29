class Mascota:
    def __init__(self, nombre, tipo, edad, raza):
        self.nombre = nombre
        self.tipo = tipo
        self.edad = edad
        self.raza = raza

cat = Mascota("Whiskers", "Gato", 3, "Siames")
print(f"Nombre: {cat.nombre}, Tipo: {cat.tipo}, Edad: {cat.edad}, Raza: {cat.raza}")
