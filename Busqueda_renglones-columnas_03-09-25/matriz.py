matriz = [
    [1,2,3],
    [4,5,6],
    [7,8,9]]

print("Matriz original:")
for i in range(3):
    for j in range(3):
        print(matriz[i][j], end=" ")
    print()

#en filas
for i in range(3):
    for j in range(3):
        print(matriz[j][i], end=" ")

#en columnas
print()
for i in range(3):
    for j in range(3):
        print(matriz[i][j], end=" ")

print()
print("fin del programa")