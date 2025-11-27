using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public GameObject root;      // Panel tienda (overlay)
    public Inventory inv;        // Inventory (Player) - con stacks (entries)
    public GameManager gm;       // GameManager

    [Header("UI (opcional)")]
    public TMP_Text headerText;

    // Botones de la tienda (autolink por nombre si no asignas)
    public Button btnBackpack, btnBall, btnMaluchan, btnAlbum, btnEnergy, btnFlashlight, btnBatteries;

    // Botón de fondo para cerrar al hacer clic fuera
    public Button backdrop; // se autoliga por nombre "Backdrop"

    void Awake()
    {
        // Autolink por nombre de hijos dentro de root
        AutoLink(ref btnBackpack,   "BtnBackpack",    BuyBackpack);
        AutoLink(ref btnBall,       "BtnBall",        BuyBall);
        AutoLink(ref btnMaluchan,   "BtnMaluchan",    BuyMaluchan);
        AutoLink(ref btnAlbum,      "BtnPhotoAlbum",  BuyAlbum);
        AutoLink(ref btnEnergy,     "BtnEnergyDrink", BuyEnergy);
        AutoLink(ref btnFlashlight, "BtnFlashlight",  BuyFlashlight);
        AutoLink(ref btnBatteries,  "BtnBatteries",   BuyBatteries);

        // Backdrop para cerrar
        AutoLink(ref backdrop, "Backdrop", Close);
    }

    void AutoLink(ref Button field, string childName, UnityEngine.Events.UnityAction onClick)
    {
        if (!field && root)
        {
            var t = root.transform.Find(childName);
            if (t) field = t.GetComponent<Button>();
        }
        if (field != null)
        {
            field.onClick.RemoveAllListeners();
            field.onClick.AddListener(onClick);
        }
    }

    void OnEnable(){ if (inv) inv.onChanged += Refresh; Refresh(); }
    void OnDisable(){ if (inv) inv.onChanged -= Refresh; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) Toggle();
        if (root.activeSelf && Input.GetKeyDown(KeyCode.Escape)) Close();
    }

    public void Toggle()
    {
        bool show = !root.activeSelf;
        root.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
        if (show) Refresh();
    }

    public void Close()
    {
        if (!root.activeSelf) return;
        root.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Refresh()
    {
        if (!root.activeSelf || inv == null || gm == null) return;

        int slots = inv.Slots;
        int used  = inv.entries.Count;
        if (headerText)
        {
            string lin = inv.flashlightTurns > 0 ? $" · Linterna:{inv.flashlightTurns}T" : "";
            headerText.text = $"Pastis: {gm.Pasti} · Slots {used}/{slots}{lin}";
        }

        // Habilitar botones según si alcanza el dinero y si cabe (stack o slot)
        SetBtn(btnBackpack,   CanBuy(50));                             // flag (no slot)
SetBtn(btnBall,       CanBuy(3)  && CanCarry(ItemType.Ball));
SetBtn(btnMaluchan,   CanBuy(7)  && CanCarry(ItemType.Maluchan));
SetBtn(btnAlbum,      CanBuy(30) && CanCarry(ItemType.PhotoAlbum));
SetBtn(btnEnergy,     CanBuy(5)  && CanCarry(ItemType.EnergyDrink));
SetBtn(btnFlashlight, CanBuy(20));                             // flag (no slot)
SetBtn(btnBatteries,  CanBuy(15) && inv.HasFlashlight && CanCarry(ItemType.Batteries));
    }

    bool CanBuy(int cost) => gm.Pasti >= cost;

    // ¿Cabe este ítem? Si es stackeable: cabe si hay stack con espacio o hay slot libre.
    bool CanCarry(ItemType it)
    {
        // No stackeables especiales (handled aparte): Mochila/Linterna
        if (it == ItemType.Backpack || it == ItemType.Flashlight) return true;

        // ¿Algún stack del mismo tipo con hueco?
        foreach (var e in inv.entries)
        {
            if (e.type == it && e.count < Inventory.MAX_STACK)
                return true;
        }
        // Si no hay hueco en stacks, necesitamos slot libre
        return inv.entries.Count < inv.Slots;
    }

    void SetBtn(Button b, bool enabled){ if (b) b.interactable = enabled; }

    // === Comprar ===
    public void BuyBackpack()   => TryBuy(ItemType.Backpack,    50, instant:false, requireFlash:false);
public void BuyBall()       => TryBuy(ItemType.Ball,         3, instant:false, requireFlash:false);
public void BuyMaluchan()   => TryBuy(ItemType.Maluchan,     7, instant:false, requireFlash:false);
public void BuyAlbum()      => TryBuy(ItemType.PhotoAlbum,  30, instant:false, requireFlash:false);
public void BuyEnergy()     => TryBuy(ItemType.EnergyDrink,  5, instant:false, requireFlash:false);
public void BuyFlashlight() => TryBuy(ItemType.Flashlight,   20, instant:false, requireFlash:false); // flag
public void BuyBatteries()  => TryBuy(ItemType.Batteries,    15, instant:false, requireFlash:true);

    void TryBuy(ItemType it, int cost, bool instant, bool requireFlash)
    {
        if (gm.Pasti < cost) return;
        if (requireFlash && !inv.HasFlashlight) return;
        if (!CanCarry(it)) return;

        // 1) Pagar
        gm.Pasti -= cost;

        // 2) Añadir al inventario (stacks o flags)
        bool added = inv.Add(it, 1);
        if (!added) { Refresh(); return; }

        // 3) Consumo instantáneo si aplica (Maluchan, Álbum, Bebida, Pilas, Linterna flag)
        if (instant)
        {
            if (it == ItemType.Flashlight)
            {
                // Flashlight en tu Inventory es flag; Add ya la activa.
            }
            else
            {
                // Buscar una entrada de ese tipo y consumir 1 unidad
                for (int i = 0; i < inv.entries.Count; i++)
                {
                    if (inv.entries[i].type == it)
                    {
                        inv.Use(i, 1); // esto aplica el efecto y descuenta del stack
                        break;
                    }
                }
            }
        }

        Refresh();
    }
}
