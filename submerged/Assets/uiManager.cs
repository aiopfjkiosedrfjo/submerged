using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Multiplayer.Center.Common.Analytics;

public class uiManager : MonoBehaviour
{
    public Canvas uiCanvas;
    public Canvas traderUICanvas;
    public TextMeshProUGUI cashDisplay;
    public TextMeshProUGUI npcDisplay;
    public npcDetector npcDetector;
    public cameraDetection cameraDetection;
    public static uiManager Instance;
    public Canvas onScreenConstantUI;
    void Start()
    {
        Instance = this;
        uiCanvas.enabled = false;
        traderUICanvas.enabled = false;
    }

    public void showInteractUI()
    {
        npcDisplay.text = "Press E to interact";
    }
    public void hideInteractUI()
    {
        npcDisplay.text = "";
    }
    public void closeAllUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        traderUICanvas.enabled = false;
        uiCanvas.enabled = false;
    }
    public void openInventoryUI()
    {
        uiCanvas.enabled = true;
    }
    public void closeInventoryUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        uiCanvas.enabled = false;
    }
    public void sellPhotos()
    {
        foreach (Image img in cameraDetection.imageDisplay)
        {
            if (img.sprite != null)
            {
                gameManager.instance.UpdateCash(10); // Sell each photo for 10 cash
                img.sprite = null; // Clear the image display
            }
        }

        updateCashDisplay();
    }
    public void closeTraderUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        traderUICanvas.enabled = false;
    }
    public void updateCashDisplay()
    {
        if (cashDisplay != null)
        {
            // Assuming there's a GameManager class that holds the player's cash
            int playerCash = gameManager.instance.playerCash;
            cashDisplay.text = playerCash.ToString();
        }
    }
}
