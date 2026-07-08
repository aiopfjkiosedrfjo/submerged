using System.Collections;
using TMPro;
using UnityEngine;

public class typeWriter : MonoBehaviour
{
    public TMP_Text dialogueText;
    public float typingSpeed = 0.05f;
    Coroutine typingCoroutine;
    public void TypeText(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeRoutine(text));
    }
    IEnumerator TypeRoutine(string text)
    {
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
