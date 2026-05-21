using System;
using System.Collections;
using Mono.Cecil.Cil;
using UnityEngine;

public class npcDetector : MonoBehaviour
{
    public LayerMask npcLayer;
    public LayerMask INTERACTABLE_OBJECT;
    public Transform playerTransform;
    public Transform teleportLocation;
    public bool anchorInteractable = false;
    public bool interactable = false;
    [System.Serializable]
    public class npcCanvasPairs
    {
        public GameObject npc;
        public Canvas npcCanvas;
    }
    public npcCanvasPairs[] npcCanvasArray;
    private GameObject targettedNPC;
    public LayerMask anchorLayer;
    public bool UIENABLED = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & npcLayer) != 0 || ((1 << other.gameObject.layer) & INTERACTABLE_OBJECT) != 0)
        {
            foreach (var pair in npcCanvasArray)
            {
                if (other.gameObject == pair.npc)
                {
                    interactable= true;
                    targettedNPC = other.gameObject;
                    break; // Exit the loop once a match is found
                }
            }
        }
        if (((1 << other.gameObject.layer) & anchorLayer) != 0)
        {
            interactable= true;
            anchorInteractable= true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is in the npcLayer
        if (((1 << other.gameObject.layer) & npcLayer) != 0 || ((1 << other.gameObject.layer) & INTERACTABLE_OBJECT) != 0)
        {
            interactable= false;
            targettedNPC = null;
            Debug.Log("NPC Left: " + other.gameObject.name);
        }
        if (((1 << other.gameObject.layer) & anchorLayer) != 0)
        {
            interactable= false;
            anchorInteractable= false;
            Debug.Log("Anchor Left: " + other.gameObject.name);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (uiManager.Instance.uiCanvas.enabled || 
                uiManager.Instance.traderUICanvas.enabled)
            {
                uiManager.Instance.closeAllUI();
                UIENABLED = false;
            }
            else if (interactable)
            {
                if (targettedNPC.layer == LayerMask.NameToLayer("NPC"))
                {
                    ShowCanvasForNPC(targettedNPC);
                    UIENABLED = true;
                    Debug.Log("this one works");
                }
                else if (targettedNPC.layer == LayerMask.NameToLayer("INTERACTABLE_OBJECT"))
                {
                    playerTransform.position = teleportLocation.position;
                    
                }
            }
            else
            {
                uiManager.Instance.openInventoryUI();
                UIENABLED = true;
            }
        }
        if (interactable && !UIENABLED)
        {
            uiManager.Instance.showInteractUI();
        }
        else
        {
            uiManager.Instance.hideInteractUI();

        }
    }
    public void ShowCanvasForNPC(GameObject npc)
    {
        bool foundNPC = false;
        foreach (var pair in npcCanvasArray)
        {
            if (pair.npc == npc)
            {
                pair.npcCanvas.enabled = true;
                foundNPC = true;
            }
            else
            {
                pair.npcCanvas.enabled = false;
            }
        }
        if (foundNPC)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
