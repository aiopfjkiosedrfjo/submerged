using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public uiManager uiManager;
    public int playerCash = 0;
    public int multiplierIncrease = 0;
    public float sanityLevel = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerCash = 100;   
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    public void UpdateCash(int amount)
    {
        playerCash += amount;
        uiManager.updateCashDisplay();
    }
    public int AddMultiplier(float distanceFromCamera)
    {
        multiplierIncrease = GetDistanceMultiplier(distanceFromCamera);
        return multiplierIncrease;
    }
    int GetDistanceMultiplier(float distanceFromCamera)
    {
        if (distanceFromCamera < 10) return 30;
        if (distanceFromCamera < 20) return 20;
        if (distanceFromCamera < 30) return 10;
        return 1;
    }
    public void spawnItem(GameObject itemPrefab, Vector3 position, Quaternion rotation)
    {
        Instantiate(itemPrefab, position, rotation);
    }
}
