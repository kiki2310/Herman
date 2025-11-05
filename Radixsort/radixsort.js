function radixSort(arr) {
    const max = Math.max(...arr);
    let exp = 1;

    while (max / exp > 0) {
        arr = countingSort(arr, exp);
        exp *= 10;
    }

    return arr;
}

function countingSort(arr, exp) {
    const n = arr.length;
    const output = new Array(n);
    const count = new Array(10).fill(0);

    for (let i = 0; i < n; i++) {
        count[Math.floor((arr[i] / exp) % 10)]++;
    }

    for (let i = 1; i < 10; i++) {
        count[i] += count[i - 1];
    }

    for (let i = n - 1; i >= 0; i--) {
        output[count[Math.floor((arr[i] / exp) % 10)] - 1] = arr[i];
        count[Math.floor((arr[i] / exp) % 10)]--;
    }

    return output;
}
const lista = [170, 45, 75, 90, 802, 24, 2, 66];
console.log("Antes de ordenar los elementos del array son:", lista);
const sortedList = radixSort(lista);
console.log("\nDespués de ordenar el arreglo:", sortedList);
