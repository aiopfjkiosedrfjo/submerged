using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using Unity.Profiling;

public class uiManager : MonoBehaviour
{
    public Canvas uiCanvas;
    public Canvas traderUICanvas;
    public Canvas traderUICanvas2;
    [SerializeField] private NotificationSO notificationSOOxygen;
    [SerializeField] private water playerOxygen;
    [SerializeField] private GameObject beaconPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Canvas importantDiscoveriesTab;
    [SerializeField] private Canvas photosTab;
    public TextMeshProUGUI cashDisplay;
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
        uiCanvas.enabled = !uiCanvas.enabled;
        if (uiCanvas.enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
            cashMultiplierIncrease += 0.75f;
            updateCashDisplay();
        }
    }
    public void IncreaseOxygenCapacity()
    {
            if (gameManager.instance.playerCash >= 100)
            {
                gameManager.instance.UpdateCash(-100);
                playerOxygen.MaxOxygen += 10f;
                NotificationManager.Instance.ShowNotification(notificationSOOxygen);
                updateCashDisplay();
            }
    }
    public void GivePlayerBeacon()
    {
        if (gameManager.instance.playerCash >= 500)
        {
            gameManager.instance.UpdateCash(-500);
            gameManager.instance.spawnItem(beaconPrefab, spawnPoint.position, spawnPoint.rotation);
            updateCashDisplay();
        }
    }
    public void SwapInventoryTabImportantDiscoveries()
    {
        photosTab.enabled = false;
        importantDiscoveriesTab.enabled = true;
    }
    public void SwapInventoryTabPhotos()
    {
        photosTab.enabled = true;
        importantDiscoveriesTab.enabled = false;
    }
    public void ZoomInToPhoto()
    {
        
    }
}
