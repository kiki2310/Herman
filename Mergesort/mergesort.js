// Combina dos subarreglos de arr[].
function merge(arr, l, m, r) {
    let n1 = m - l + 1;
    let n2 = r - m;

    // Crea arreglos temporales
    let L = new Array(n1);
    let R = new Array(n2);

    // Copia los datos a los arreglos temporales
    for (let i = 0; i < n1; i++)
        L[i] = arr[l + i];
    for (let j = 0; j < n2; j++)
        R[j] = arr[m + 1 + j];

    // Combina los arreglos temporales de nuevo en arr[l..r]
    let i = 0; // Índice inicial del primer subarreglo
    let j = 0; // Índice inicial del segundo subarreglo
    let k = l; // Índice inicial del subarreglo combinado

    while (i < n1 && j < n2) {
        if (L[i] <= R[j]) {
            arr[k] = L[i];
            i++;
        } else {
            arr[k] = R[j];
            j++;
        }
        k++;
    }

    // Copia los elementos restantes de L[], si hay alguno
    while (i < n1) {
        arr[k] = L[i];
        i++;
        k++;
    }

    // Copia los elementos restantes de R[], si hay alguno
    while (j < n2) {
        arr[k] = R[j];
        j++;
        k++;
    }
}

// l es para el índice izquierdo y r es para el índice derecho del subarreglo de arr a ser ordenado
function mergeSort(arr, l, r) {
    if (l >= r) {
        return; // Condición de parada de la recursión
    }
    let m = l + Math.floor((r - l) / 2);
    mergeSort(arr, l, m);
    mergeSort(arr, m + 1, r);
    merge(arr, l, m, r);
}

// Código de ejecución
let arr = [39, 28, 44, 11];
console.log("Arreglo antes de ordenar:");
console.log(arr.join(' '));

mergeSort(arr, 0, arr.length - 1);

console.log("\nArreglo después de ordenar:");
console.log(arr.join(' '));
