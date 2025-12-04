

import java.util.ArrayList;
import java.util.List;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;

/**
 * Árbol Binario de Búsqueda (BST) para claves enteras.
 */
public class BinarySearchTree {

    private Nodo root;

    public BinarySearchTree() {
        this.root = null;
    }

    // ================== INSERCIÓN ==================

    /**
     * Inserta una clave en el BST.
     * @param key clave a insertar
     * @return true si se insertó, false si ya existía
     */
    public boolean insert(int key) {
        if (root == null) {
            root = new Nodo(key);
            return true;
        }
        boolean inserted = insertRec(root, key);
        return inserted;
    }

    private boolean insertRec(Nodo current, int key) {
        if (key == current.key) {
            // No permitir duplicados
            return false;
        } else if (key < current.key) {
            if (current.left == null) {
                current.left = new Nodo(key);
                return true;
            }
            return insertRec(current.left, key);
        } else {
            if (current.right == null) {
                current.right = new Nodo(key);
                return true;
            }
            return insertRec(current.right, key);
        }
    }

    // ================== BÚSQUEDA ==================

    /**
     * Verifica si una clave está en el BST.
     */
    public boolean contains(int key) {
        return searchNode(key) != null;
    }

    private Nodo searchNode(int key) {
        Nodo current = root;
        while (current != null) {
            if (key == current.key) {
                return current;
            } else if (key < current.key) {
                current = current.left;
            } else {
                current = current.right;
            }
        }
        return null;
    }

    /**
     * Devuelve la ruta desde la raíz hasta el nodo con la clave dada.
     * Si no se encuentra, devuelve el camino hasta donde se pudo llegar.
     */
    public List<Integer> searchPath(int key) {
        List<Integer> path = new ArrayList<>();
        Nodo current = root;
        while (current != null) {
            path.add(current.key);
            if (key == current.key) {
                return path;
            } else if (key < current.key) {
                current = current.left;
            } else {
                current = current.right;
            }
        }
        // no encontrado, pero devolvemos el camino recorrido
        return path;
    }

    // ================== ELIMINACIÓN ==================

    /**
     * Elimina una clave del BST.
     * Maneja: hoja, un hijo, dos hijos.
     * @return true si se eliminó, false si la clave no existía.
     */
    public boolean delete(int key) {
        if (!contains(key)) {
            return false;
        }
        root = deleteRec(root, key);
        return true;
    }

    private Nodo deleteRec(Nodo current, int key) {
        if (current == null) return null;

        if (key < current.key) {
            current.left = deleteRec(current.left, key);
        } else if (key > current.key) {
            current.right = deleteRec(current.right, key);
        } else {
            // Nodo encontrado: casos
            // 1) Sin hijos
            if (current.left == null && current.right == null) {
                return null;
            }
            // 2) Un solo hijo
            else if (current.left == null) {
                return current.right;
            } else if (current.right == null) {
                return current.left;
            }
            // 3) Dos hijos: tomar el sucesor (mínimo del subárbol derecho)
            else {
                int minRight = findMin(current.right);
                current.key = minRight;
                current.right = deleteRec(current.right, minRight);
            }
        }
        return current;
    }

    private int findMin(Nodo node) {
        Nodo current = node;
        while (current.left != null) {
            current = current.left;
        }
        return current.key;
    }

    // ================== RECORRIDOS ==================

    public List<Integer> inorder() {
        List<Integer> result = new ArrayList<>();
        inorderRec(root, result);
        return result;
    }

    private void inorderRec(Nodo node, List<Integer> result) {
        if (node == null) return;
        inorderRec(node.left, result);
        result.add(node.key);
        inorderRec(node.right, result);
    }

    public List<Integer> preorder() {
        List<Integer> result = new ArrayList<>();
        preorderRec(root, result);
        return result;
    }

    private void preorderRec(Nodo node, List<Integer> result) {
        if (node == null) return;
        result.add(node.key);
        preorderRec(node.left, result);
        preorderRec(node.right, result);
    }

    public List<Integer> postorder() {
        List<Integer> result = new ArrayList<>();
        postorderRec(root, result);
        return result;
    }

    private void postorderRec(Nodo node, List<Integer> result) {
        if (node == null) return;
        postorderRec(node.left, result);
        postorderRec(node.right, result);
        result.add(node.key);
    }

    // ================== ALTURA Y TAMAÑO ==================

    /**
     * Altura del árbol (número de niveles).
     * Árbol vacío: 0. Raíz sola: 1.
     */
    public int height() {
        return heightRec(root);
    }

    private int heightRec(Nodo node) {
        if (node == null) return 0;
        int hl = heightRec(node.left);
        int hr = heightRec(node.right);
        return Math.max(hl, hr) + 1;
    }

    /**
     * Número de nodos del árbol.
     */
    public int size() {
        return sizeRec(root);
    }

    private int sizeRec(Nodo node) {
        if (node == null) return 0;
        return 1 + sizeRec(node.left) + sizeRec(node.right);
    }

    // ================== EXPORTAR INORDEN ==================

    /**
     * Exporta el recorrido inorden a un archivo de texto (una clave por línea).
     */
    public void exportInorder(String filename) throws IOException {
        List<Integer> in = inorder();
        try (PrintWriter writer = new PrintWriter(new FileWriter(filename))) {
            for (int key : in) {
                writer.println(key);
            }
        }
    }

    // ================== PRUEBAS RECOMENDADAS ==================

    /**
     * Ejecuta los casos de prueba sugeridos.
     * Imprime resultados en consola.
     */
    public static void runCasosPrueba() {
        BinarySearchTree bst = new BinarySearchTree();

        int[] secuencia = {45, 15, 79, 90, 10, 55, 12, 20, 50};
        for (int v : secuencia) {
            bst.insert(v);
        }

        System.out.println("=== CASOS DE PRUEBA ===");
        System.out.println("Inorden esperado: 10 12 15 20 45 50 55 79 90");
        System.out.println("Inorden actual  : " + bst.inorder());

        // Búsquedas
        System.out.println("Buscar 20 (debe existir): " + bst.contains(20));
        System.out.println("Buscar 100 (no existe): " + bst.contains(100));

        // Eliminar hoja 90
        bst.delete(90);
        System.out.println("Inorden tras eliminar 90: " + bst.inorder());

        // Eliminar nodo con un hijo 79
        bst.delete(79);
        System.out.println("Inorden tras eliminar 79: " + bst.inorder());

        // Eliminar nodo con dos hijos 45
        bst.delete(45);
        System.out.println("Inorden tras eliminar 45: " + bst.inorder());

        System.out.println("Altura actual: " + bst.height());
        System.out.println("Tamaño actual: " + bst.size());
        System.out.println("=== FIN CASOS DE PRUEBA ===");
    }
}

