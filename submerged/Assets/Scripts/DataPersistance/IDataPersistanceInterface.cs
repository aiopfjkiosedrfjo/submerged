using UnityEngine;

public interface IDataPersistanceInterface
{
    void LoadData(gameData data);
    void SaveData(ref gameData data);
}
