using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableCard draggable = eventData.pointerDrag.GetComponent<DraggableCard>();

        if (draggable != null)
        {
            draggable.transform.SetParent(transform); // Move card into the drop zone
            draggable.transform.localPosition = Vector3.zero; // Center it in the slot
        }
    }
}