using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public CardPlacementState cardPlacementState;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;

        if (droppedCard != null)
        {
            droppedCard.transform.SetParent(transform, true); 
            droppedCard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
            droppedCard.transform.SetAsLastSibling();

            if (cardPlacementState != null)
            {
                cardPlacementState.PlaceCard(droppedCard.gameObject); ;
            }
            else
            {
                Debug.LogError("Draggable card is not assigned in drop zone");
            }
            
        }
    }
}