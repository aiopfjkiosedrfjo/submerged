using UnityEngine;

[System.Serializable]
public class gameData 
{
    public int playerCash;
    public float OxygenLevel;
    public gameData()
    {
        this.OxygenLevel = 65f;
        this.playerCash = 0;
    }
}
