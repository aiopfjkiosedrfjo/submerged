using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
public enum SanityState
{
    Stable,
    Anxious,
    Hallucinating,
    Paranoid,
    Insane
}
public class sanityLevelEvents : MonoBehaviour
{
    public bool FishAllLookAtPlayer = false;
    [SerializeField] private float HowFastDoesSanityDecrease = 1f;
    private bool fishAllLookAtPlayerActive= true;
    public float sanityLevel = 100f;
    private SanityState currentSanityState = SanityState.Stable;
    [SerializeField] private water waterScript;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private Transform doorSpawnPoint;
    [SerializeField] private Player player;
    [Header("Lady In Red Spawn")]
    [SerializeField] private List<Transform> spawnLocations = new List<Transform>();
    [SerializeField] private GameObject ladyInRedPrefab;
    [Header("Trash Room")]
    [SerializeField] private Transform trashPileTeleport;
    [SerializeField] private CinemachineVirtualCamera trashRoomCamera;
    [SerializeField] private CinemachineVirtualCamera MainCamera;
    [SerializeField] private playercam playercam;
    public float TimeTakenForLadyInRedToDisappear = 10f;
    public float SanityLevel
    {
        get
        {
            return sanityLevel;
        }
        set
        {
            sanityLevel = Mathf.Clamp(value, 0f, 100f);
            CheckSanityTier();
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckSanityLevel();
        CheckSanityTier();
    }
    private void CheckSanityTier()
    {
        SanityState newState = sanityLevel switch
        {
            >= 75f => SanityState.Stable,
            >= 50f => SanityState.Anxious,
            >= 25f => SanityState.Hallucinating,
            >= 10f => SanityState.Paranoid,
            _ => SanityState.Insane
        };
        if (newState != currentSanityState)
        {
            currentSanityState = newState;
            TriggerSanityEvent(newState);
        }
    }
    private void TriggerSanityEvent(SanityState state)
    {
        switch (state)
        {
            case SanityState.Stable:
                Debug.Log("Sanity is stable.");
                break;
            case SanityState.Anxious:
                Debug.Log("Player is feeling anxious.");
                SpawnLadyInRed();
                break;
            case SanityState.Hallucinating:
                Debug.Log("Player is hallucinating.");
                break;
            case SanityState.Paranoid:
                Debug.Log("Player is paranoid.");
                break;
            case SanityState.Insane:
                SpawnTheDoor(doorSpawnPoint.position, doorSpawnPoint.rotation, player);
                Debug.Log("Player is insane.");
                break;
        }
    }
    void CheckSanityLevel()
    {
        if (waterScript.inWater)
        {
            float depthPercentage = waterScript.depth / 100f;
            float exponentialFactor = Mathf.Pow(depthPercentage, 2);
            sanityLevel -= Time.deltaTime * exponentialFactor * HowFastDoesSanityDecrease;
        }
        else
        {
            sanityLevel += Time.deltaTime / 10f;
        }
    }
    public void SpawnTheDoor(Vector3 position, Quaternion rotation, Player player)
    {
        Vector3 rayStart = new Vector3(position.x, position.y + 50f, position.z);
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Vector3 spawnPosition = hit.point + Vector3.up * 4f;
                GameObject door = Instantiate(doorPrefab, spawnPosition, rotation);
                doorTeleport doorScript = door.GetComponentInChildren<doorTeleport>();
                doorScript.SetPlayer(player);
                door.transform.LookAt(new Vector3(player.transform.position.x, door.transform.position.y, player.transform.position.z));
                door.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
                door.transform.Rotate(0f, 90f, 0f);
            }
        }
    }
    public void SpawnLadyInRed()
    {
        float shortestDistance = Mathf.Infinity;
        Transform closestObject = null;
        for (int i = 0; i < spawnLocations.Count; i++)
        {
            float distanceToPlayer = Vector3.Distance(spawnLocations[i].position, player.gameObject.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                closestObject = spawnLocations[i];
            }
        }
        StartCoroutine(InstantiateLadyInRed(closestObject, player));
    }
    private System.Collections.IEnumerator InstantiateLadyInRed(Transform spawnPoint, Player player)
    {
        GameObject ladyInRed = Instantiate(ladyInRedPrefab, spawnPoint.position, spawnPoint.rotation);
        ladyInRed ladyInRedScript = ladyInRed.GetComponent<ladyInRed>();
        ladyInRedScript.PassPlayer(player);
        yield return new WaitForSeconds(TimeTakenForLadyInRedToDisappear);
        Destroy(ladyInRed);
    }
    public void TeleportToTrashRoom()
    {
        player.rb.linearVelocity = Vector3.zero;
        player.rb.position = trashPileTeleport.position;
        StartCoroutine(changeTrashRoomCameras());
    }
    private IEnumerator changeTrashRoomCameras()
    {
        timelineManager.SwitchCamera(trashRoomCamera);
        playercam.yRotation = 180f;
        yield return new WaitForSeconds(3f);
        timelineManager.SwitchCamera(MainCamera);
        
    }
}
