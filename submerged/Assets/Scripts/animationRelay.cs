using UnityEngine;

public class animationRelay : MonoBehaviour
{
    [SerializeField] private playerInteract playerInteract;
    private void TriggerParentFunction()
    {
        playerInteract.OpenInventoryUI();
    }
}
