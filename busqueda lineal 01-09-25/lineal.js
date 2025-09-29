const readline = require('readline');
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

function busquedaLineal(arreglo, objetivo) {
    for (let i = 0; i < arreglo.length; i++) {
        if (arreglo[i] === objetivo) {
            return i; // Retorna el índice si se encuentra el elemento
        }
    }
    return -1; // Retorna -1 si el elemento no se encuentra
}

function imprimirArreglo(arreglo) {
    // Usamos .join() para un formato más limpio en JS
    console.log("Arreglo: " + arreglo.join(' '));
}

function insertarNumero(arreglo, numero, posicion) {
    // Validamos que la posición esté dentro de los límites del arreglo
    if (posicion < 0 || posicion >= arreglo.length) {
        console.log("Posición inválida.");
        return;
    }
    arreglo[posicion] = numero;
}

const arreglo = [10, 20, 30, 40, 50];
imprimirArreglo(arreglo);

// Creamos una función asíncrona para poder usar await
async function main() {
    try {
        const objetivoStr = await new Promise(resolve => rl.question('Introduce el elemento a buscar: ', resolve));
        const objetivo = parseInt(objetivoStr);

        // Validamos que la entrada sea un número
        if (isNaN(objetivo)) {
            console.log("Error: Debes introducir un número válido.");
            return;
        }

        const indice = busquedaLineal(arreglo, objetivo);
        if (indice !== -1) {
            console.log(`Elemento encontrado en el índice: ${indice}`);
        } else {
            console.log("Elemento no encontrado.");
        }

        const nuevoValorStr = await new Promise(resolve => rl.question('Valor para insertar: ', resolve));
        const nuevoValor = parseInt(nuevoValorStr);

        const posicionStr = await new Promise(resolve => rl.question('Posición para insertar: ', resolve));
        const posicion = parseInt(posicionStr);

        // Validamos que las entradas para insertar sean números
        if (isNaN(nuevoValor) || isNaN(posicion)) {
            console.log("Error: El valor y la posición deben ser números válidos.");
            return;
        }

        insertarNumero(arreglo, nuevoValor, posicion);
        imprimirArreglo(arreglo);

    } catch (error) {
        console.error("Ocurrió un error:", error);
    } finally {
        // Nos aseguramos de cerrar readline al final
        rl.close();
    }
}

// Llamamos a nuestra función principal para que se ejecute
main();