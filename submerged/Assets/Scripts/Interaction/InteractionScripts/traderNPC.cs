using UnityEngine;

public class traderNPC : MonoBehaviour, IInteractable, IToggleable
{
    [SerializeField]private Canvas traderCanvas;
    [Header("Scripts to Disable")]
    [SerializeField]private Player playerMovement;
    [SerializeField]private playercam playerCam;
    [SerializeField]private cameraDetection cameraDetection;
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.enabled = false;
        playerCam.enabled = false;
        cameraDetection.enabled = false;
    }
    public void Hide()
    {
        isOpen = false;
        traderCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.enabled = true;
        playerCam.enabled = true;
        cameraDetection.enabled = true;
    }
}
