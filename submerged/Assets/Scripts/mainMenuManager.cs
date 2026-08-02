using UnityEngine;
using UnityEngine.SceneManagement;
public enum MenuState
{
    TitleScreen,
    OptionsMenu,
    NewGameMenu
}
public class mainMenuManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoadName;
    [SerializeField] private Canvas titleScreenCanvas;
    [SerializeField] private Canvas optionsMenuCanvas;
    [SerializeField] private Canvas newGameMenuCanvas;
    [SerializeField] private GameObject confirmationMenu;
    private void Start()
    {
        SetState(MenuState.TitleScreen);
    }
    public void NewGame()
    {
        dataPersistanceManager.instance.state = DataPersistenceState.NewGame;
        SceneManager.LoadScene(this.sceneToLoadName);
    }
    public void LoadGame()
    {
        
            dataPersistanceManager.instance.state = DataPersistenceState.LoadGame;
            SceneManager.LoadScene(this.sceneToLoadName);
    }
    public void SetState(MenuState state)
    {
        titleScreenCanvas.enabled = state == MenuState.TitleScreen;
        optionsMenuCanvas.enabled = state == MenuState.OptionsMenu;
        newGameMenuCanvas.enabled = state == MenuState.NewGameMenu;
    }
    public void OpenTitleScreen()
    {
        SetState(MenuState.TitleScreen);

    }
    public void OpenOptionsMenu()
    {
        SetState(MenuState.OptionsMenu);
    }
    public void OpenNewGameMenu()
    {
        SetState(MenuState.NewGameMenu);
    }
    public void OpenConfirmationMenu()
    {
        confirmationMenu.SetActive(true);
    }
    public void CloseConfirmationMenu()
    {
        confirmationMenu.SetActive(false);
    }
}
