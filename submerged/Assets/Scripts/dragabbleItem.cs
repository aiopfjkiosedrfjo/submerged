using UnityEngine;
using UnityEngine.EventSystems;

public class draggableItem : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentCanvas.worldCamera == null) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, 
            eventData.position, 
            parentCanvas.worldCamera, 
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }
}