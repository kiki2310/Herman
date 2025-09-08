def busqueda_lineal(arreglo, objetivo):
    for i in range(len(arreglo)):
        if arreglo[i] == objetivo:
            return i  # Retorna el índice si se encuentra el elemento
    return -1  # Retorna -1 si el elemento no se encuentra

def imprimir_arreglo(arreglo):
    print("Arreglo:", end=" ")
    for num in arreglo:
        print(num, end=" ")
    print()

def insertar_numero(arreglo, numero, posicion):
    if posicion < 0 or posicion >= len(arreglo):
        print("Posición inválida.")
        return
    arreglo[posicion] = numero

if __name__ == "__main__":
    arreglo = [10, 20, 30, 40, 50]
    imprimir_arreglo(arreglo)

    objetivo = int(input("Introduce el elemento a buscar: "))
    indice = busqueda_lineal(arreglo, objetivo)

    if indice != -1:
        print(f"Elemento encontrado en el índice: {indice}")
    else:
        print("Elemento no encontrado.")

    nuevo_valor = int(input("Valor para insertar: "))
    posicion = int(input("Posición para insertar: "))
    insertar_numero(arreglo, nuevo_valor, posicion)
    imprimir_arreglo(arreglo)

    print("Fin del programa.")