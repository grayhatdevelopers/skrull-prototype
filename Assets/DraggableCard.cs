using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler ,IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 originalPosition; // Store original position
    private Transform originalParent; // Store original parent
    private Vector3 ogPosition;
    public float hoverHeight = 30f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvas = GetComponentInParent<Canvas>(); // Get the UI Canvas
        originalPosition = rectTransform.anchoredPosition; // Store initial position
        originalParent = transform.parent; // Store initial parent
        
        ogPosition = transform.localPosition;
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Move the card up when hovered
        transform.localPosition = ogPosition + new Vector3(0, hoverHeight, 0);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the card to its original position when the mouse exits
        transform.localPosition = ogPosition;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Make it slightly transparent while dragging
        canvasGroup.blocksRaycasts = false; // Allows it to be ignored by raycasts while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // Move card with pointer
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Restore visibility
        canvasGroup.blocksRaycasts = true; // Enable raycasts again

        if (transform.parent == originalParent) // If still in the same parent, return to original position
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}