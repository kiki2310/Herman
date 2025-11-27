using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("Panel del Inventario")]
    public GameObject inventoryPanel;  // Arrastra aquí tu InventoryPanel

    private bool isOpen = true;

    void Start()
    {
        // Empieza visible o invisible según prefieras
        inventoryPanel.SetActive(isOpen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
        }
    }
}
