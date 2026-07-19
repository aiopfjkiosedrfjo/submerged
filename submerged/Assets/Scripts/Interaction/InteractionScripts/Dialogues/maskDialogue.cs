using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class maskDialogue : MonoBehaviour, IInteractable
{
    [SerializeField]private DialogueSO dialogueSO;
    [SerializeField]private DialogueSO dialogueSO2;
    [SerializeField]private DialogueSO dialogueSO3;
    [SerializeField]private typeWriter typeWriter;
    [SerializeField]private GameObject dialogueBox;
    [Header("Masks")]
    [SerializeField] private int dialogueCount = 0;
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
        playerPressed = false;
        int currentDialogue = gameManager.instance.HowManyTimesHaveTheyEnteredMaskRoom;
        DialogueSO currentDialogueSO = null;
        switch(currentDialogue)
        {
            case 0:
                currentDialogueSO = dialogueSO;
                break;
            case 1:
                currentDialogueSO = dialogueSO2;
                break;
            case 2:
                currentDialogueSO = dialogueSO3;
                break;
        }


        dialogueBox.SetActive(true);
        if (currentDialogueSO != null)
            StartCoroutine(DisplayDialogue(currentDialogueSO));
    }
    IEnumerator DisplayDialogue(DialogueSO currentDialogueLine)
    {
        for (int i = 0; i < currentDialogueLine.dialogueStrings.Count;)
        {
            typeWriter.TypeText(currentDialogueLine.dialogueStrings[i]);
            while (!playerPressed)
                yield return null;
            i++;
            playerPressed = false;
        }
        TriggerEvent();
        Hide();
    }
    public void Hide()
    {
        dialogueBox.SetActive(false);
    }
    private void TriggerEvent()
    {
        if (gameManager.instance.HowManyTimesHaveTheyEnteredMaskRoom == 2)
        {
            gameManager.instance.MaskEvent(false, true);
            return;
        }
        gameManager.instance.MaskEvent(false, false);

    }
}
