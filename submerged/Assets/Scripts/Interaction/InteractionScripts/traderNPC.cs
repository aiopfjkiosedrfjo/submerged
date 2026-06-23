using UnityEngine;

public class traderNPC : MonoBehaviour, IInteractable, IToggleable
{
    [SerializeField]private Canvas traderCanvas;
    private bool isOpen = false;
    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(playerInteract playerInteract)
    {
        if (isOpen) Hide();
        else Show();
        return true;
    }
    public void Show()
    {
        isOpen = true;
        traderCanvas.enabled = true;
    }
    public void Hide()
    {
        isOpen = false;
        traderCanvas.enabled = false;
    }
}
