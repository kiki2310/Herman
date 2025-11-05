def merge(a, l, m, r):
    # Merge dos subarreglos de a[].
    a1 = m - l + 1   # Tamaño del primer subarreglo
    a2 = r - m       # Tamaño del segundo subarreglo

    # Crear arrays temporales
    L = [0] * a1     # Crear array temporal izquierdo
    R = [0] * a2     # Crear array temporal derecho

    # Copiar datos a los arrays temporales L[] y R[]
    for j in range(0, a1):
        L[j] = a[l + j]   # copiar datos al array temporal izquierdo
    for k in range(0, a2):
        R[k] = a[m + 1 + k]  # copiar datos al array temporal derecho

    i = 0  # Índice inicial del primer subarreglo
    j = 0  # Índice inicial del segundo subarreglo
    k = l  # Índice inicial del subarreglo mezclado

    # Mezclar los arrays temporales de nuevo en a[l..r]
    while i < a1 and j < a2:  # recorrer ambos arrays
        if L[i] <= R[j]:      # comparar los elementos de ambos arrays
            a[k] = L[i]       # copiar el elemento más pequeño al array original
            i = i + 1         # incrementar el índice del primer array
        else:
            a[k] = R[j]       # copiar el elemento más pequeño al array original
            j = j + 1         # incrementar el índice del segundo array
        k = k + 1             # incrementar el índice del array original

    # Copiar los elementos restantes de L[], si hay alguno
    while i < a1:
        a[k] = L[i]
        i = i + 1
        k = k + 1

    # Copiar los elementos restantes de R[], si hay alguno
    while j < a2:
        a[k] = R[j]
        j = j + 1
        k = k + 1


# l es para el índice izquierdo, y r es para el índice derecho del subarreglo de 'a' a ser ordenado
def mergeSort(a, l, r):
    # Función principal que ordena a[l..r]
    if l < r:
        # Igual que (l + r) // 2, pero evita el desbordamiento para grandes valores de l y r
        m = l + (r - 1) // 2

        # Ordenar la primera y segunda mitad
        mergeSort(a, l, m)       # ordenar la primera mitad
        mergeSort(a, m + 1, r)   # ordenar la segunda mitad
        merge(a, l, m, r)        # mezclar las dos mitades


# Código para probar la implementación de MergeSort
a = [39, 28, 44, 11]   # arreglo desordenado
s = len(a)             # tamaño del arreglo

print("Antes de ordenar el arreglo: ")  # imprime el arreglo
for j in range(s):
    print("%d" % a[j], end=" ")

mergeSort(a, 0, s - 1)  # llama a la función mergeSort

print("\nDespués de ordenar el arreglo: ")
for j in range(s):
    print("%d" % a[j], end=" ")
