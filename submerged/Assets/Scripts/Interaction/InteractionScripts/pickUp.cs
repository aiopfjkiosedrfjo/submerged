using UnityEngine;

public class pickUp : MonoBehaviour, IInteractable, IToggleable
{
    public bool isTryingToPickUp =false;
    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(playerInteract playerInteract)
    {
        
        return true;
    }
    public void Show()
    {
        
    }
    public void Hide()
    {
        
    }

}
