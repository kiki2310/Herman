function burbuja(arr) {
    let n = arr.length;
    let vaso;
    for (let i = 0; i < n - 1; i++) {
        for (let j = 0; j < n - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                // Intercambia arr[j] y arr[j+1]
                vaso = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = vaso;
            }
        }
    }
    return arr;
}

arreglo = [64, 34, 25, 12, 22, 11, 90];
console.log("Arreglo original: " + arreglo);
console.log("Arreglo ordenado: " + burbuja(arreglo));