// Función de ordenamiento por inserción
function insertionSort(bukt) {
    for (let j = 1; j < bukt.length; j++) {
        const val = bukt[j];
        let k = j - 1;
        while (k >= 0 && bukt[k] > val) {
            bukt[k + 1] = bukt[k];
            k--;
        }
        bukt[k + 1] = val;
    }
    return bukt;
}

// Función principal de ordenamiento bucket
function bucketSort(inputArr) {
    const s = inputArr.length;
    if (s === 0) {
        return inputArr;
    }
    
    // Crear lista de buckets vacíos
    const bucketArr = Array.from({ length: s }, () => []);

    // Colocar cada elemento en su bucket
    for (let j of inputArr) {
        const bi = Math.floor(s * j);
        const index = bi === s ? s - 1 : bi; // Asegurar índice
        bucketArr[index].push(j);
    }

    // Ordenar cada bucket
    for (let bukt of bucketArr) {
        insertionSort(bukt);
    }

    // Concatenar los buckets en el arreglo original
    let idx = 0;
    for (let bukt of bucketArr) {
        for (let j of bukt) {
            inputArr[idx++] = j;
        }
    }
    
    return inputArr;
}

// Ejemplo de uso
const inputArr = [0.77, 0.16, 0.38, 0.25, 0.71, 0.93, 0.22, 0.11, 0.24, 0.67];

console.log("Arreglo antes de ordenar:");
console.log(inputArr.join(" "));

bucketSort(inputArr);

console.log("Arreglo después de ordenar:");
console.log(inputArr.join(" "));