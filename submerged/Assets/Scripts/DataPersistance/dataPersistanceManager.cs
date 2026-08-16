using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Audio;
public enum DataPersistenceState
{
    NewGame,
    LoadGame,
}
public class dataPersistanceManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private gameData _gameData;
    public float volume;
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
    private void Update()
    {
        SetMixerVolume(volume);
    }
    public void SetMixerVolume(float Volume)
    {
        if (Volume <= 0)
        {
            audioMixer.SetFloat("masterVolume", -80f);
            return;
        }
        float decibelValue = Mathf.Log10(Volume) * 20f;
        audioMixer.SetFloat("masterVolume", decibelValue);
    }
    public void NewGame()
    {
        this._gameData = new gameData();
        this.dataPersistanceInterfaces = FindAllDataPersistanceObjects();
    }
    public void LoadGame()
    {
        this.dataPersistanceInterfaces = FindAllDataPersistanceObjects();
        this._gameData = this.dataHandler.Load();
        if (this._gameData == null)
        {
            Debug.LogWarning("No data was found. NewGameStarted");
            NewGame();
            return;
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
    public void Volume(float Volume)
    {
        
    }
}
