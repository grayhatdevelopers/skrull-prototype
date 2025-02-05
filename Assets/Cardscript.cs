using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 originalPosition; // Store original position
    private Transform originalParent; // Store original parent

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