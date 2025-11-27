using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // ===== Config =====
    public const int MAX_STACK = 64;

    [Header("Capacidad")]
    public int baseSlots = 5;

    [Header("Refs")]
    public GameManager gm;         // arrastra GameManager
    public PlayerController pc;    // arrastra PlayerController

    [Header("Linterna / Powerups")]
    public int flashlightTurns = 0;
    private bool ownedFlashlight = false;
    private bool ownedBackpack  = false;

    public bool HasFlashlight => ownedFlashlight;
    public bool HasBackpack   => ownedBackpack;

    public int Slots => HasBackpack ? 8 : baseSlots;

    // Notifica a la UI (InventoryUI/ShopUI)
    public Action onChanged;

    // ===== Modelo con stacks =====
    [Serializable]
    public class InvEntry
    {
        public ItemType type;
        public int count;
        public InvEntry(ItemType t, int c){ type = t; count = c; }
    }
    public List<InvEntry> entries = new();

    // ---- Helpers
    bool CanStack(ItemType it) =>
        it != ItemType.Flashlight && it != ItemType.Backpack && it != ItemType.Treasure;

    bool HasSpaceForNewEntry() => entries.Count < Slots;

    // ---- Añadir (respeta stacks y límites)
    public bool Add(ItemType it, int qty = 1)
    {
        if (qty <= 0) return false;

        // Flags que NO ocupan slot
        if (it == ItemType.Backpack)
        {
            if (!ownedBackpack){ ownedBackpack = true; onChanged?.Invoke(); }
            return true;
        }
        if (it == ItemType.Flashlight)
        {
            ownedFlashlight = true;
            flashlightTurns = Mathf.Max(flashlightTurns, 3);
            onChanged?.Invoke();
            return true;
        }

        // Stackeables
        if (CanStack(it))
        {
            // 1) Intenta rellenar pilas existentes
            foreach (var e in entries)
            {
                if (e.type != it) continue;
                int free = MAX_STACK - e.count;
                if (free <= 0) continue;
                int take = Mathf.Min(free, qty);
                e.count += take;
                qty -= take;
                if (qty == 0){ onChanged?.Invoke(); return true; }
            }
            // 2) Crea nuevas entradas
            while (qty > 0)
            {
                if (!HasSpaceForNewEntry()){ onChanged?.Invoke(); return false; }
                int take = Mathf.Min(MAX_STACK, qty);
                entries.Add(new InvEntry(it, take));
                qty -= take;
            }
            onChanged?.Invoke();
            return true;
        }

        // NO stackeables (Tesoro, etc.)
        if (!HasSpaceForNewEntry()) return false;
        entries.Add(new InvEntry(it, 1));
        onChanged?.Invoke();
        return true;
    }

    // ---- Usar (consume del stack)
    public bool Use(int index, int qty = 1)
    {
        if (index < 0 || index >= entries.Count || qty <= 0) return false;
        var e = entries[index];
        bool anyUsed = false;

        for (int i = 0; i < qty; i++)
        {
            if (!UseItem(e.type)) break; // aplica efecto 1 vez
            e.count--;
            anyUsed = true;
            if (e.count <= 0){ entries.RemoveAt(index); break; }
        }

        if (anyUsed) onChanged?.Invoke();
        return anyUsed;
    }

    // ---- Efectos de cada objeto
    public bool UseItem(ItemType it)
    {
        switch (it)
        {
            case ItemType.Ball:
                if (pc != null)
                {
                    pc.AwaitBallTarget = true;
                    gm?.banner?.Show("Elige una casilla con clic", 1.2f);
                    return true; // se consume ya
                }
                return false;

            case ItemType.Maluchan:
                if (gm != null){ gm.Energy = Mathf.Min(gm.MaxEnergy, gm.Energy + 1); return true; }
                return false;

            case ItemType.PhotoAlbum:
                if (gm != null){ gm.AddHp(+1); return true; }
                return false;

            case ItemType.EnergyDrink:
                if (pc != null){ pc.pendingDash = true; return true; }
                return false;

            case ItemType.Batteries:
                if (!HasFlashlight) return false;
                flashlightTurns += 3;
                return true;

            case ItemType.Flashlight:
            case ItemType.Backpack:
                // No se "usan" desde el stack: son flags al recogerlos
                return false;

            case ItemType.Treasure:
                // Coleccionable: no tiene uso
                return false;

            default:
                return false;
        }
    }

    // ---- Tirar (libera espacio)
    public bool Drop(int index, int qty = 1)
    {
        if (index < 0 || index >= entries.Count || qty <= 0) return false;
        var e = entries[index];
        int take = Mathf.Min(qty, e.count);
        e.count -= take;
        if (e.count <= 0) entries.RemoveAt(index);
        onChanged?.Invoke();
        // Si quieres “tirarlo al mundo”, instancia un prefab aquí.
        return true;
    }

    // ---- Turnos (linterna)
    public void OnTurnEnded()
    {
        if (flashlightTurns > 0)
        {
            flashlightTurns--;
            onChanged?.Invoke();
        }
    }
}