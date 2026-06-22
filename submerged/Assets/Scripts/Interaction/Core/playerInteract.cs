using UnityEngine;

public class playerInteract : MonoBehaviour
{
    [SerializeField] private float castDistance = 10f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Transform rayOrigin;
    private Outline currentOutline;

    private void Update()
    {
        if(DoInteractionTest(out IInteractable interactable, out Outline outline))
        {
            if (currentOutline != null && currentOutline != outline)
            {
                currentOutline.enabled = false;
            }
            currentOutline = outline;
            if (interactable.CanInteract())
            {
                if (currentOutline != null)
                    currentOutline.enabled = true;
                
                if (Input.GetKeyDown(KeyCode.V))
                    interactable.Interact(this);
            }
            else
            {
                if(currentOutline != null)
                    currentOutline.enabled = false;
            }
        }
        else
        {
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }
    }
    private bool DoInteractionTest(out IInteractable interactable, out Outline outline)
    {
        interactable = null;
        outline = null;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, castDistance, interactableLayers))
        {
            interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                outline = hitInfo.collider.GetComponent<Outline>();
                return true;
            }
            return false;
        }
        return false;
    }
}
