

import java.io.IOException;
import java.util.List;
import java.util.Scanner;

/**
 * Gestor de números con Árbol Binario de Búsqueda en consola.
 * Comandos:
 *  insert, search, delete, inorder, preorder, postorder,
 *  height, size, export, help, test, exit
 */
public class GestorNumerosBST {

    public static void main(String[] args) {
        BinarySearchTree bst = new BinarySearchTree();
        Scanner scanner = new Scanner(System.in);

        System.out.println("==============================================");
        System.out.println(" Gestor de números con Árbol Binario de Búsqueda");
        System.out.println("==============================================");
        mostrarHelp();

        while (true) {
            System.out.print("\n> ");
            String comando = scanner.nextLine().trim().toLowerCase();

            switch (comando) {
                case "insert":
                    System.out.print("Número a insertar: ");
                    Integer numIns = leerEntero(scanner);
                    if (numIns == null) break;
                    boolean inserted = bst.insert(numIns);
                    if (inserted) {
                        System.out.println("Insertado: " + numIns);
                    } else {
                        System.out.println("La clave " + numIns + " ya existe en el árbol.");
                    }
                    break;

                case "search":
                    System.out.print("Número a buscar: ");
                    Integer numBus = leerEntero(scanner);
                    if (numBus == null) break;
                    List<Integer> path = bst.searchPath(numBus);
                    if (path.isEmpty()) {
                        System.out.println("El árbol está vacío.");
                    } else {
                        System.out.println("Ruta recorrida: " + path);
                        if (bst.contains(numBus)) {
                            System.out.println("Resultado: el número " + numBus + " SÍ se encuentra en el árbol.");
                        } else {
                            System.out.println("Resultado: el número " + numBus + " NO se encuentra en el árbol.");
                        }
                    }
                    break;

                case "delete":
                    System.out.print("Número a eliminar: ");
                    Integer numDel = leerEntero(scanner);
                    if (numDel == null) break;
                    boolean deleted = bst.delete(numDel);
                    if (deleted) {
                        System.out.println("Eliminado: " + numDel);
                    } else {
                        System.out.println("La clave " + numDel + " no existe en el árbol.");
                    }
                    break;

                case "inorder":
                    imprimirRecorrido("Inorden", bst.inorder());
                    break;

                case "preorder":
                    imprimirRecorrido("Preorden", bst.preorder());
                    break;

                case "postorder":
                    imprimirRecorrido("Posorden", bst.postorder());
                    break;

                case "height":
                    System.out.println("Altura del árbol: " + bst.height());
                    break;

                case "size":
                    System.out.println("Número de nodos: " + bst.size());
                    break;

                case "export":
                    System.out.print("Nombre del archivo (ej. inorder.txt): ");
                    String filename = scanner.nextLine().trim();
                    try {
                        bst.exportInorder(filename);
                        System.out.println("Recorrido inorden exportado a '" + filename + "'");
                    } catch (IOException e) {
                        System.out.println("Error al escribir el archivo: " + e.getMessage());
                    }
                    break;

                case "help":
                    mostrarHelp();
                    break;

                case "test":
                    // Ejecuta los casos de prueba recomendados
                    BinarySearchTree.runCasosPrueba();
                    break;

                case "exit":
                    System.out.println("Saliendo del programa. ¡Hasta luego!");
                    scanner.close();
                    return;

                default:
                    System.out.println("Comando no reconocido. Escribe 'help' para ver la lista de comandos.");
                    break;
            }
        }
    }

    private static void mostrarHelp() {
        System.out.println("\nComandos disponibles:");
        System.out.println("  insert   - Insertar número");
        System.out.println("  search   - Buscar número y mostrar ruta");
        System.out.println("  delete   - Eliminar número");
        System.out.println("  inorder  - Mostrar recorrido inorden");
        System.out.println("  preorder - Mostrar recorrido preorden");
        System.out.println("  postorder- Mostrar recorrido posorden");
        System.out.println("  height   - Mostrar altura del árbol");
        System.out.println("  size     - Mostrar número de nodos");
        System.out.println("  export   - Exportar inorden a archivo de texto");
        System.out.println("  test     - Ejecutar casos de prueba");
        System.out.println("  help     - Mostrar esta ayuda");
        System.out.println("  exit     - Salir");
    }

    private static void imprimirRecorrido(String nombre, List<Integer> lista) {
        if (lista.isEmpty()) {
            System.out.println("El árbol está vacío.");
        } else {
            System.out.println(nombre + ": " + lista);
        }
    }

    /**
     * Lee un entero del scanner. Si hay error, devuelve null y muestra mensaje.
     */
    private static Integer leerEntero(Scanner scanner) {
        String linea = scanner.nextLine().trim();
        try {
            return Integer.parseInt(linea);
        } catch (NumberFormatException e) {
            System.out.println("Entrada inválida, debe ser un número entero.");
            return null;
        }
    }
}
