using UnityEngine;

public class drivingBoat : MonoBehaviour, IInteractable
{
    [SerializeField] private Player player;
    
    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(playerInteract interactor)
    {
        player.isRidingBoat = !player.isRidingBoat;
        return true;
    }
}
