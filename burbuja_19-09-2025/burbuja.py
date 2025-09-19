def burbuja(lista):
    n = len(lista)
    for i in range(n):
        for j in range(0, n-i-1):
            if lista[j] > lista[j+1]:
                lista[j], lista[j+1] = lista[j+1], lista[j]
    return lista

if __name__ == "__main__":
    numeros = [64, 34, 25, 12, 22, 11, 90]
    print("Lista original:", numeros)
    sorted_numeros = burbuja(numeros)
    print("Lista ordenada:", sorted_numeros)
    