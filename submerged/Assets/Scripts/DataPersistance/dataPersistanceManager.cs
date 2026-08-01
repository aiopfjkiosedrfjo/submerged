using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class dataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private gameData _gameData;
    public static dataPersistanceManager instance { get; private set;}
    private List<IDataPersistanceInterface> dataPersistanceInterfaces;
    private FileDataHandler dataHandler;
    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, this.fileName);
        Debug.Log(Application.persistentDataPath);
        this.dataPersistanceInterfaces = FindAllDataPersistanceObjects();
        this._gameData = new gameData();
    }

    private void Awake()
    {
        if (instance != null)
            Debug.Log("more then one data persistance manaer");
        instance = this;
    }
    public void NewGame()
    {
        this._gameData = new gameData();
    }
    public void LoadGame()
    {
        this._gameData = this.dataHandler.Load();
        if (this._gameData == null)
        {
            Debug.Log("no data to load");
        }
        foreach (IDataPersistanceInterface dataPersistanceInterface in this.dataPersistanceInterfaces)
        {
            dataPersistanceInterface.LoadData(this._gameData);
        }
    }
    public void SaveGame()
    {
        foreach (IDataPersistanceInterface dataPersistanceInterface in this.dataPersistanceInterfaces)
        {
            dataPersistanceInterface.SaveData(ref this._gameData);
        }
        dataHandler.Save(this._gameData);
    }
    private List<IDataPersistanceInterface> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistanceInterface> dataPersistanceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistanceInterface>();
        return new List<IDataPersistanceInterface>(dataPersistanceObjects);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
