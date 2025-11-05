class ShellSort {
    // Una función de utilidad para imprimir un arreglo
    static displayArr(inputArr) {
        console.log(inputArr.join(" "));
    }

    // Función para ordenar inputArr usando Shell Sort
    sort(inputArr) {
        const size = inputArr.length;
        // Comienza con un espacio grande y luego reduce el espacio
        for (let gapSize = Math.floor(size / 2); gapSize > 0; gapSize = Math.floor(gapSize / 2)) {
            
            // Realiza un ordenamiento por inserción con espacios
            for (let j = gapSize; j < size; j++) {
                let val = inputArr[j];
                let k = j;

                // Compara y mueve elementos mientras el anterior (a una distancia 'gapSize') sea mayor
                while (k >= gapSize && inputArr[k - gapSize] > val) {
                    inputArr[k] = inputArr[k - gapSize];
                    k -= gapSize;
                }

                // Inserta el elemento en su posición correcta
                inputArr[k] = val;
            }
        }
    }
}

// Bloque principal
const inputArr = [36, 34, 43, 11, 15, 20, 28, 45];
console.log("Arreglo antes de ser ordenado:");
ShellSort.displayArr(inputArr);

const obj = new ShellSort();
obj.sort(inputArr);

console.log("Arreglo después de ser ordenado:");
ShellSort.displayArr(inputArr);
