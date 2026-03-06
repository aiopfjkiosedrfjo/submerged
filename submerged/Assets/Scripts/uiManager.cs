using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    public Canvas uiCanvas;
    public Canvas traderUICanvas;
    public TextMeshProUGUI cashDisplay;
    public TextMeshProUGUI npcDisplay;
    public npcDetector npcDetector;
    public cameraDetection cameraDetection;
    public static uiManager Instance;
    public float cashToBeUpdated;
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
        foreach (cameraDetection.PhotoData data in cameraDetection.photoDataList)
        {
            foreach (Image img in cameraDetection.imageDisplay)
            {
                if (img.sprite != null)
                {
                    cashToBeUpdated = (data.multiplierIncrease/100f)+1;
                    cashToBeUpdated *= 10;
                    gameManager.instance.UpdateCash((int)cashToBeUpdated); 
                    img.sprite = null; 
                }
            }
        }
        cameraDetection.photoDataList.Clear();
        cameraDetection.count = 0;
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
            int playerCash = gameManager.instance.playerCash;
            cashDisplay.text = playerCash.ToString();
        }
    }
}
