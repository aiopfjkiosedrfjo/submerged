using UnityEngine;

[System.Serializable]
public class gameData 
{
    public int playerCash;
    public float OxygenLevel;
    public int NumberOfDives;
    public int HowManyTimesHaveTheyEnteredMaskRoom;
    public gameData()
    {
        this.OxygenLevel = 65f;
        this.playerCash = 0;
        this.NumberOfDives = 0;
        this.HowManyTimesHaveTheyEnteredMaskRoom = 0;
    }
}
