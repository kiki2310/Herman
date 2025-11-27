using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    public Inventory inv;
    public int index;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inv == null) return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            inv.Drop(index, 1);
        }
    }
}
