using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class finalHintDialogue : MonoBehaviour, IInteractable
{
    [SerializeField]private DialogueSO dialogueSO;
    [SerializeField]private typeWriter typeWriter;
    [SerializeField] private Canvas tutorialCanvas;
    [SerializeField]private GameObject dialogueBox;
    [SerializeField] private cameraDetection cameraScript;
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
        return true;
    }
    private void StartDialogue()
    {
        cameraScript.enabled = false;
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
        cameraScript.enabled = true;
        Hide();
    }
    public void Hide()
    {
        dialogueBox.SetActive(false);
    }
}
