using UnityEngine;

public class playerInteract : MonoBehaviour
{
    [SerializeField] private float castDistance = 10f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Transform rayOrigin;


    private void Update()
    {
        if(DoInteractionTest(out IInteractable interactable))
        {
            if (interactable.CanInteract())
            {
                if (Input.GetKeyDown(KeyCode.V))
                    interactable.Interact(this);
            }
        }
    }
    private bool DoInteractionTest(out IInteractable interactable)
    {
        interactable = null;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, castDistance, interactableLayers))
        {
            interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {

                return true;
            }
            return false;
        }
        return false;
    }
}
