using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private void Start()
    {
        // Delay excecuting load game until main scene has loaded
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
