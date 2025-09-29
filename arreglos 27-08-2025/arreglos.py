# En Python, el tipo de dato más similar a un arreglo es la 'lista' (list).
# Las listas son ordenadas, mutables (se pueden cambiar) y pueden contener elementos de diferentes tipos.

# 1. Declaración e inicialización
# Una lista vacía
colores = []

# Una lista con elementos iniciales
numeros = [1, 2, 3, 4, 5]
print(f"Lista inicial de números: {numeros}")

# 2. Acceder a elementos
# Los índices comienzan en 0
primer_numero = numeros[0]  # Accede al primer elemento (1)
print(f"El primer número es: {primer_numero}")

# 3. Modificar elementos
numeros[2] = 10  # Cambia el tercer elemento (3) a 10
print(f"Lista modificada: {numeros}")

# 4. Añadir elementos
# .append() añade un elemento al final de la lista
numeros.append(6)
print(f"Lista después de append(6): {numeros}")

# 5. Longitud de la lista
print(f"La longitud de la lista es: {len(numeros)}")

# 6. Iterar sobre una lista
print("Iterando con un bucle for:")
for numero in numeros:
    print(f"- {numero}")

