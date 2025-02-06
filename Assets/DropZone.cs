using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableCard draggable = eventData.pointerDrag.GetComponent<DraggableCard>();

        if (draggable != null)
        {
            draggable.transform.SetParent(transform, true); // Keep UI scaling correct
            draggable.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Center the card
            draggable.transform.SetAsLastSibling(); // Bring the card to the front
        }
    }
}