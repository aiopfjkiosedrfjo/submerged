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
    void Start()
    {
        uiCanvas.enabled = false;
        traderUICanvas.enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && uiCanvas.enabled)
        {
            uiCanvas.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && !uiCanvas.enabled)
        {
            uiCanvas.enabled = true;
            updateCashDisplay();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gameManager.instance.UpdateCash(50);
            updateCashDisplay();
        }
        if (npcDetector.interactable)
        {
            npcDisplay.enabled = true;
        }
        else
        {
            npcDisplay.enabled = false;
        }

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
        foreach (Renderer rend in cameraDetection.photoTargets)
        {
            rend.gameObject.SetActive(true); // Reactivate the photo targets
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
