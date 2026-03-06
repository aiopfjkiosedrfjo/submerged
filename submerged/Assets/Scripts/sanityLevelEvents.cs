using UnityEngine;

public class sanityLevelEvents : MonoBehaviour
{
    public bool FishAllLookAtPlayer = false;
    private bool fishAllLookAtPlayerActive= true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckSanityLevel();
    }
    void CheckSanityLevel()
    {
        fishAllLookAtPlayerActive = false;
        if (gameManager.instance.sanityLevel <= 50 && fishAllLookAtPlayerActive == true)
        {
            FishAllLookAtPlayer = true;
            fishAllLookAtPlayerActive = false;
            ResetTriggers();
        }
        else
        {
            FishAllLookAtPlayer = false;

        }
    }
    public void ResetTriggers()
    {
        fishAllLookAtPlayerActive = true;
    }
}
