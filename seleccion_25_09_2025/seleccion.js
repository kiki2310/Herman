function seleccion(arr) {
    let n = arr.length;
    let minIndex, vaso;
    for (let i = 0; i < n - 1; i++) {
        minIndex = i;
        for (let j = i + 1; j < n; j++) {
            if (arr[j] < arr[minIndex]) {
                minIndex = j;
            }
        }
        if (minIndex !== i) {
            vaso = arr[i];
            arr[i] = arr[minIndex];
            arr[minIndex] = vaso;
        }
    }
    return arr;
}
let arreglo = [64, 25, 12, 22, 11];
console.log("Arreglo original: " + arreglo);
console.log("Arreglo ordenado: " + seleccion(arreglo));