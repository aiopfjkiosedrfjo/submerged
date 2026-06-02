using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    public Canvas uiCanvas;
    public Canvas traderUICanvas;
    public Canvas traderUICanvas2;
    public TextMeshProUGUI cashDisplay;
    public TextMeshProUGUI npcDisplay;
    public npcDetector npcDetector;
    public cameraDetection cameraDetection;
    public static uiManager Instance;
    public float cashToBeUpdated;
    public float cashMultiplierIncrease;
    void Start()
    {
        Instance = this;
        uiCanvas.enabled = false;
        traderUICanvas.enabled = false;
        traderUICanvas2.enabled = false;
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
        traderUICanvas2.enabled = false;
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
        for (int i = 0; i < cameraDetection.photoDataList.Count; i++)
        {
            var data = cameraDetection.photoDataList[i];
            var img = cameraDetection.imageDisplay[i];

            if (img.sprite != null)
            {
                float cashToBeUpdated = (data.multiplierIncrease / 10f) + cashMultiplierIncrease;
                cashToBeUpdated *= 10;
                if (data.speciesName.Count > 0)
                {
                    cashToBeUpdated *= data.speciesName.Count;
                }
                Debug.Log("Fish No." + i + " Multiplier: " + data.multiplierIncrease + " Species Count: " + data.speciesName.Count + " Total Cash: " + cashToBeUpdated);
                gameManager.instance.UpdateCash((int)cashToBeUpdated);

                img.sprite = null;
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
        traderUICanvas2.enabled = false;
    }
    public void updateCashDisplay()
    {
        if (cashDisplay != null)
        {
            int playerCash = gameManager.instance.playerCash;
            cashDisplay.text = playerCash.ToString();
        }
    }
    public void IncreaseCashMultiplier()
    {
        if (gameManager.instance.playerCash >= 100)
        {
            gameManager.instance.UpdateCash(-100);
            cashMultiplierIncrease += 0.1f;
            updateCashDisplay();
        }
    }
}
