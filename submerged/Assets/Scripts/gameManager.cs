using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public Player playerScript;
    public uiManager uiManager;
    public sanityLevelEvents sanityLevelScript;
    public water waterScript;
    public int playerCash = 0;
    public int multiplierIncrease = 0;

    public float sanityLevel = 100f;
    [Header("Mask Event")]
    public List<GameObject> gameObjects = new List<GameObject>();
    public List<GameObject> spotLights = new List<GameObject>();
    public GameObject eyesInTheDark;
    public float eyesStaringDuration = 1f;
    public int HowManyTimesHaveTheyEnteredMaskRoom = 0;
    public GameObject fishPile;
    public GameObject hole;
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
    public void MaskEvent(bool resetOrActivate, bool ignoreFade)
    {
        foreach (GameObject game in gameObjects)
        {
            game.SetActive(resetOrActivate);

        }
        foreach (GameObject spotLight in spotLights)
        {
            spotLight.SetActive(false);
        }
        if (HowManyTimesHaveTheyEnteredMaskRoom == 1) eyesInTheDark.SetActive(!resetOrActivate);
        if (!resetOrActivate && !ignoreFade)
        {
            StartCoroutine(Delay());
            fishPile.SetActive(false);
        }
        else
        {
            HowManyTimesHaveTheyEnteredMaskRoom ++;
        }
        if (ignoreFade)
        {
            fishPile.SetActive(true);
            hole.SetActive(false);
        }
    }
    private System.Collections.IEnumerator Delay()
    {
        yield return new WaitForSeconds(5);
        waterScript.ExternalScriptsTriggerFadeOut(false, eyesStaringDuration);
        yield return new WaitForSeconds(eyesStaringDuration);
        playerScript.rb.position = playerScript.boatTeleport.position;
        playerScript.rb.linearVelocity = Vector3.zero;
        MaskEvent(true, false);
    }

}
