let arreglos = [];
let dato;
const readline = require("readline");
const red = readline.createInterface({
  input: process.stdin,
  output: process.stdout,
  prompt: "miniexamen> ",
});

for(let i=0; i <10; i++){
 dato= red("Ingrese un dato");
 arreglos.push(dato);
}

console.log(arreglos);
