using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class triggerManager : MonoBehaviour
{
    public static triggerManager instance;
    [Header("SpotLightTrigger")]
    [SerializeField] private GameObject spotLight1;
    [SerializeField] private GameObject spotLight2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource MusicAudioSource;
    [SerializeField] private AudioClip spotLightSFX;
    [SerializeField] private water waterScript;
    [SerializeField] private GameObject cutSceneCanvas;
    [SerializeField] private Animator insturctionAnimator;
    [SerializeField] private Player player;
    [Header("Chase Sequence")]
    [SerializeField]private float spawnRadius = 100f;
    [SerializeField]private GameObject footStepsPrefab;
    [SerializeField] private GameObject breathSpot;
    [SerializeField]private GameObject mask;
    private chasePlayer footsteps;
    private void Awake()
    {
        instance = this;
    }
    
    public void Trigger(triggerEnum trigger)
    {
        switch (trigger)
        {
            case triggerEnum.SpotLightTrigger1:
            SpotLightTrigger(spotLight1);
            break;
            case triggerEnum.SpotLightTrigger2:
            SpotLightTrigger(spotLight2);
            break;
            case triggerEnum.RedRoomFadeOut:
            StartCoroutine(RedRoomFadeOut());
            break;
            case triggerEnum.instructionPopup:
            insturctionAnimator.SetTrigger("insturctionsPopup");
            break;

        }
    }
    public void TriggerExit(triggerExitEnum exitTrigger)
    {
        switch (exitTrigger)
        {
            case triggerExitEnum.BeginChaseSequence:
            TriggerChaseSequence();
            break;
        }
    }
    public void SpotLightTrigger(GameObject game)
    {
        if (game != null)
        {
            game.SetActive(true);
            audioSource.PlayOneShot(spotLightSFX);
        }
    }
    public System.Collections.IEnumerator RedRoomFadeOut()
    {
        waterScript.ExternalScriptsTriggerFadeOut(false, 5f);
        yield return new WaitForSeconds(5f);
        cutSceneCanvas.SetActive(true);
        
    }
    public void TriggerChaseSequence()
    {
        gameManager.instance.MaskEvent(false, true);
        MusicAudioSource.Play();
        SpawnObjectAroundPlayer();
    }
    public void SpawnObjectAroundPlayer()
    {
        if (player == null) return;
        Vector2 randomCirclePoint = UnityEngine.Random.insideUnitCircle;
        Vector3 spawnDirection = new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        Vector3 spawnPosition = player.transform.position + (spawnDirection * spawnRadius);
        GameObject footsteps = Instantiate(footStepsPrefab, spawnPosition, Quaternion.identity);
        chasePlayer chaseScript = footsteps.GetComponent<chasePlayer>();
        chaseScript.breathTP = breathSpot;
    }
    public void EndChaseSequence()
    {
        Destroy(footsteps);
    }
    public void enableMask()
    {
        mask.SetActive(true);
    }
}
