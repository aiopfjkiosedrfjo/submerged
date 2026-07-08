using UnityEngine;
using TMPro;
public class playerInteract : MonoBehaviour
{

    //BUGS:
    //Need to update the script disabling to be part of a state machine so its easier to manage and less buggy

    [SerializeField] private float castDistance = 10f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private CanvasGroup interactionText;
    [SerializeField] private KeyCode inputKey = KeyCode.E;
    [Header("Scripts to Disable")]
    [SerializeField] private playercam playercam;
    [SerializeField] private cameraDetection cameraDetection;
    [SerializeField] private Animator animator;
    private bool InventoryIsOpen = false;



    private void Update()
    {
        if(DoInteractionTest(out IInteractable interactable))
        {
            if (InventoryIsOpen)
            {
                if (Input.GetKeyDown(inputKey))
                {
                    OpenInventoryAnimation();
                    gameManager.instance.UpdateCash(0);
                }
                return;
            }
            if (interactable.CanInteract())
            {
                interactionText.alpha = 1f;
                if (Input.GetKeyDown(inputKey))
                    interactable.Interact(this);
            }
        }
        else
        {
            if (Input.GetKeyDown(inputKey))
            {
                OpenInventoryAnimation();
            }
            interactionText.alpha = 0f;
                
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
    private void OpenInventoryAnimation()
    {
        if (!InventoryIsOpen)
            animator.SetTrigger("pullOut");
            gameManager.instance.UpdateCash(0);
        if (InventoryIsOpen)
            animator.SetTrigger("pullBack");
    }
    public void OpenInventoryUI()
    {
        playercam.enabled = !playercam.enabled;
        cameraDetection.enabled = !cameraDetection.enabled;
        uiManager.Instance.openInventoryUI();
        InventoryIsOpen = !InventoryIsOpen;
    }
}
