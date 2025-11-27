using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Refs")]
    public Inventory inv;           // Player (Inventory con stacks)
    public GameManager gm;          // GameManager
    public Transform contentRoot;   // panel contenedor (InventoryPanel)
    public TMP_Text infoText;       // “Slots 2/3 · Linterna: 2T” (opcional)

    [Header("Prefabs")]
    public Button slotPrefab;       // PREFAB del Project con Button en la raíz
    public TMP_FontAsset font;      // PressStart2P
    public Material fontMat;        // material Outline

    void Awake()
    {
        if (!contentRoot) contentRoot = transform;
    }

    void OnEnable()
    {
        if (inv) inv.onChanged += Rebuild;
        Rebuild();
    }
    void OnDisable()
    {
        if (inv) inv.onChanged -= Rebuild;
    }

    void Update()
    {
        if (inv == null) return;
        int count = inv != null ? inv.entries.Count : 0;

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && i < count)
                inv.Use(i, 1);
        }
    }

    public void Rebuild()
    {
        if (!slotPrefab)
        {
            Debug.LogError("InventoryUI: slotPrefab no asignado (usa un PREFAB del Project, no un objeto en escena).");
            return;
        }
        if (contentRoot == null) return;
        if (inv == null) return;

        // Limpia hijos actuales (una sola vez)
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
            Destroy(contentRoot.GetChild(i).gameObject);

        int slots = inv.Slots;
        int count = inv.entries.Count;

        // Crear botones para entradas actuales
        for (int i = 0; i < count; i++)
        {
            var e = inv.entries[i];

            var btn = Instantiate(slotPrefab, contentRoot);
            btn.transform.localScale = Vector3.one;   // <-- importante con Layouts
            btn.name = $"Slot_{i + 1}";

            // Texto: nombre + xN si el stack > 1
            string label = ShortName(e.type);
            if (e.count > 1) label += $" x{e.count}";
            var txt = SetupSlotText(btn, label);

            // Clicks: izq = usar 1, der = tirar 1 (vía SlotUI)
            var slot = btn.GetComponent<SlotUI>();
            if (!slot) slot = btn.gameObject.AddComponent<SlotUI>();
            slot.inv = inv;
            slot.index = i;

            btn.onClick.RemoveAllListeners();
            int idx = i;
            btn.onClick.AddListener(() => inv.Use(idx, 1));
        }

        // Huecos vacíos
        for (int i = count; i < slots; i++)
        {
            var btn = Instantiate(slotPrefab, contentRoot);
            btn.transform.localScale = Vector3.one;   // <-- igual aquí
            btn.name = $"Empty_{i + 1}";
            SetupSlotText(btn, "—");
            btn.interactable = false;
        }

        // Info de arriba
        if (infoText)
        {
            string lin = inv.flashlightTurns > 0 ? $" · Linterna:{inv.flashlightTurns}T" : "";
            infoText.text = $"Slots {count}/{slots}{lin}";
            if (font) infoText.font = font;
            if (fontMat) infoText.fontSharedMaterial = fontMat;
        }
    }

    TMP_Text SetupSlotText(Button btn, string text)
    {
        var txt = btn.GetComponentInChildren<TMP_Text>();
        if (!txt)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(TMP_Text));
            go.transform.SetParent(btn.transform, false);
            txt = go.GetComponent<TMP_Text>();
            txt.alignment = TextAlignmentOptions.Center;
        }
        txt.text = text;
        if (font) txt.font = font;
        if (fontMat) txt.fontSharedMaterial = fontMat;
        txt.fontSize = 28;
        return txt;
    }

    string ShortName(ItemType it) => it switch
    {
        ItemType.Backpack => "Mochila",
        ItemType.Ball => "Pelota",
        ItemType.Maluchan => "Maluchan",
        ItemType.PhotoAlbum => "Álbum",
        ItemType.EnergyDrink => "Bebida",
        ItemType.Flashlight => "Linterna",
        ItemType.Batteries => "Pilas",
        ItemType.Treasure => "Tesoro",
        _ => it.ToString()
    };
}
