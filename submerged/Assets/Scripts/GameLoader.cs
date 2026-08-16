using UnityEngine;
using UnityEngine.Audio;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        if (dataPersistanceManager.instance == null) return;
        if (dataPersistanceManager.instance.state == DataPersistenceState.NewGame)
        {
            if (dataPersistanceManager.instance == null)
            {
                return;
            }
            else
            {
                dataPersistanceManager.instance.NewGame();
            }
        }
        //Delay loading game 
        if (dataPersistanceManager.instance.state == DataPersistenceState.LoadGame)
        {
            if (dataPersistanceManager.instance == null)
            {
                return;
            }
            else
            {
                dataPersistanceManager.instance.LoadGame();
            }
        }
    }

}
