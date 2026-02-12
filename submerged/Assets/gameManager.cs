using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public uiManager uiManager;
    public int playerCash = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerCash = 100;   // Starting cash
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
}
