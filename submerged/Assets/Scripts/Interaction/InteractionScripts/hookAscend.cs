using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class hookAscend : MonoBehaviour, IInteractable
{
    [SerializeField] private float ascendingSpeed = 10f;
    [Header("Scripts to Disable")]
    [SerializeField] private Player player;
    [SerializeField] private Transform topPosition;
    [SerializeField] private sanityLevelEvents sanityLevelScript;
    private bool isBeingLifted = false;
    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(playerInteract interactor)
    {
        StartLift();
        return true;
    }
    private void FixedUpdate()
    {
        if (isBeingLifted)
        {
            LiftPlayer();
        }
    }
    private void LiftPlayer()
    {
        player.rb.linearVelocity = Vector3.zero;
        float distance = topPosition.position.y - player.transform.position.y;
        if (distance > 0.1f)
        {
            player.rb.linearVelocity = Vector3.up * ascendingSpeed;
            player.canMove = false;
        }
        else
        {
            player.rb.linearVelocity = Vector3.zero;
            
            isBeingLifted = false;
            player.canMove = true;
            
            CheckIfSanityEventOccurs();
        }
    }
    private void StartLift()
    {
        gameManager.instance.NumberOfDives ++;
        isBeingLifted = true;
    }
    public void CheckIfSanityEventOccurs()
    {
        switch (gameManager.instance.NumberOfDives)
        {
            case 3 or 5 or 8:
            sanityLevelScript.TeleportToTrashRoom();
            break;
            default:
            player.gameObject.transform.position = topPosition.position;
            break;
        }
    }
}
