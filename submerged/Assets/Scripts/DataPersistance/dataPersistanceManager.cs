using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public enum DataPersistenceState
{
    NewGame,
    LoadGame,
}
public class dataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private gameData _gameData;
    public DataPersistenceState state;
    public static dataPersistanceManager instance { get; private set;}
    private List<IDataPersistanceInterface> dataPersistanceInterfaces;
    private FileDataHandler dataHandler;
    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, this.fileName);
        Debug.Log(Application.persistentDataPath);
        this.dataPersistanceInterfaces = FindAllDataPersistanceObjects();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void NewGame()
    {
        this._gameData = new gameData();
        this.dataPersistanceInterfaces = FindAllDataPersistanceObjects();
    }
    public void LoadGame()
    {
        if (this._gameData == null)
        {
            Debug.LogWarning("No data was found. NewGameStarted");
            NewGame();
            return;
        }
        this.dataPersistanceInterfaces = FindAllDataPersistanceObjects();
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
        this.dataPersistanceInterfaces = FindAllDataPersistanceObjects();
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
