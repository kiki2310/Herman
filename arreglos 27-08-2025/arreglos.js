
let frutas = [];

// Un arreglo con elementos iniciales
let numeros = [1, 2, 3, 4, 5];
console.log("Arreglo inicial de números:", numeros);

// 2. Acceder a elementos
// Los índices de los arreglos comienzan en 0
let primerNumero = numeros[0]; // Accede al primer elemento (1)
console.log("El primer número es:", primerNumero);

// 3. Modificar elementos
numeros[2] = 10; // Cambia el tercer elemento (3) a 10
console.log("Arreglo modificado:", numeros);

// 4. Añadir elementos
// .push() añade un elemento al final del arreglo
numeros.push(6);
console.log("Arreglo después de push(6):", numeros);

// 5. Longitud del arreglo
console.log("La longitud del arreglo es:", numeros.length);

// 6. Iterar sobre un arreglo
console.log("Iterando con un bucle for:");
for (let i = 0; i < numeros.length; i++) {
  console.log(`Elemento en el índice ${i}: ${numeros[i]}`);
}

console.log("Iterando con forEach:");
numeros.forEach(function(numero, indice) {
  console.log(`Elemento en el índice ${indice}: ${numero}`);
});

