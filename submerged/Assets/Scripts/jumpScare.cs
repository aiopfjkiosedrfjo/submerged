using System.Collections;
using UnityEngine;

public class jumpScare : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform scaryObject;
    [SerializeField] private GameObject ScaryGameObject;
    [SerializeField] private AudioSource aud;
    [SerializeField] private AudioClip jumpscareNoise;
    [SerializeField] private LayerMask targettedLayers;
    [SerializeField]private Animator animator;
    [Range (0f, 1f)]
    public float viewAccuracy = 0.95f;
    public float maxDetectionDistance =15f;
    public bool hasTriggeredJumpscare = false;
    private void Update()
    {
        if (hasTriggeredJumpscare) return;
        Vector3 directionToObject = (scaryObject.position - playerCamera.position).normalized;
        float dotProduct = Vector3.Dot(playerCamera.forward, directionToObject);
        if (dotProduct > viewAccuracy)
        {
            float distance = Vector3.Distance(playerCamera.position, scaryObject.position);
            if (distance <= maxDetectionDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDetectionDistance, targettedLayers))
                {
                    TriggerJumpscare();
                }
            }
        }
    }
    public void TriggerJumpscare()
    {
        hasTriggeredJumpscare = true;
        aud.PlayOneShot(jumpscareNoise);
        animator.SetTrigger("jumpscare");
        StartCoroutine(DelayJumpscare());
    }
    public IEnumerator DelayJumpscare()
    {
        yield return new WaitForSeconds(1f);
        ScaryGameObject.SetActive(false);
        Debug.Log("settin gameobject off");
        triggerManager.instance.EndChaseSequence();
    }
}
