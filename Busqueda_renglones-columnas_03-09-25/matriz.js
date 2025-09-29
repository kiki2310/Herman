matriz = [
    [1, 2, 3],
    [4, 5, 6],
    [7, 8, 9]
];
console.log("--- Matriz Original ---");
for (let i = 0; i < matriz.length; i++) {
    let fila = "";
    for (let j = 0; j < matriz[i].length; j++) {
        fila += matriz[i][j] ;
    }
    console.log(fila);
}
console.log("\n--- columnas ---");
for (let i = 0; i < matriz[0].length; i++) {
    let columna = 0;
    for (let j = 0; j < matriz.length; j++) {
        columna = matriz[j][i];
process.stdout.write(columna + " ");
    }
}
console.log("\n--- filas ---");
for (let i = 0; i < matriz.length; i++) {
    let fila = 0;
    for (let j = 0; j < matriz[i].length; j++) {
        fila = matriz[i][j] ;
            process.stdout.write(fila + " ");
    }

}


console.log("\nfin del programa");