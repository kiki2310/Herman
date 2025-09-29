def quicksort(lista):

    if len(lista) <= 1:
        return lista
    else:
        pivote = lista.pop()

        menores = []
        mayores = []

        for elemento in lista:
            if elemento < pivote:
                menores.append(elemento)
            else:
                mayores.append(elemento)

        return quicksort(menores) + [pivote] + quicksort(mayores)

if __name__ == "__main__":
    numeros = [64, 34, 25, 12, 22, 11, 90, 5]
    print("Lista original:", numeros)
    lista_ordenada = quicksort(numeros)
    print("Lista ordenada con Quicksort:", lista_ordenada)
