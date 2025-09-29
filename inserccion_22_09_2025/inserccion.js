function inserccion(arr) {
    let n = arr.length;
    let lim;
    for (let i = 1; i < n; i++) {
        lim = arr[i];
        let j = i - 1;
        while (j >= 0 && arr[j] > lim) {
            arr[j + 1] = arr[j];
            j--;
        }
        arr[j + 1] = lim;
    }
}
arreglo = [64, 34, 25, 12, 22, 11, 90];
console.log("Arreglo original: " + arreglo);
inserccion(arreglo);
console.log("Arreglo ordenado: " + arreglo);