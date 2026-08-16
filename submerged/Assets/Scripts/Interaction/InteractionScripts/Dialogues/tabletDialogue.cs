using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class tabletDialogue : MonoBehaviour, IInteractable
{
    [SerializeField]private DialogueSO dialogueSO;
    [SerializeField]private typeWriter typeWriter;
    [SerializeField] private Canvas tutorialCanvas;
    [SerializeField]private GameObject dialogueBox;
    [Header("Scripts to Disable")]
    [SerializeField]private Player playerMovement;
    [SerializeField]private playercam playerCam;
    [SerializeField]private cameraDetection cameraDetection;
    public bool playerPressed = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerPressed = true;
        }
    }
    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(playerInteract interactor)
    {
        StartDialogue();
        AudioSource aud = GetComponent<AudioSource>();
        aud.volume = 0f;
        return true;
    }
    private void StartDialogue()
    {
        cameraDetection.enabled = false;
        dialogueBox.SetActive(true);
        StartCoroutine(DisplayDialogue());
    }
    IEnumerator DisplayDialogue()
    {
        for (int i = 0; i < dialogueSO.dialogueStrings.Count;)
        {
            typeWriter.TypeText(dialogueSO.dialogueStrings[i]);
            while (!playerPressed)
                yield return null;
            i++;
            playerPressed = false;
        }
        PlayTutorial();
        Hide();
    }
    public void Hide()
    {
        dialogueBox.SetActive(false);
    }
    public void PlayTutorial()
    {
        tutorialCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerMovement.enabled = false;
        playerCam.enabled = false;
        cameraDetection.enabled = false;
    }
}
