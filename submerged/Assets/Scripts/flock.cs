using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class flock : MonoBehaviour
{
    float speed;
    public float minimumspeed = 2f;
    public float maximumspeed = 6f;
    public float rotationSpeed = 4.0f;
    public float fleeingSpeed = 20f;
    public float neighbourDistance = 6.0f;
    float speedChangeTimer;
    bool turning = false;
    public sanityLevelEvents sanityLevelEventManager;
    public GameObject player;
    bool AllFishLookingAtPlayerTriggered = false;
    public bool lookingAtPlayer = false;
    public globalFlock globalFlockScript;
    public Outline outline;
    public SkinnedMeshRenderer objectRenderer;
    public ParticleSystem particleEffect;
    public float fadeDuration = 2f;
    public globalFlock flockManagerScript;
    private Vector3 goalpos;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 3f;
    [SerializeField] private float groundAvoidanceStrength = 5f;
    [Header("HuntingFish")]
    [SerializeField] private float huntDistance =10f;
    [SerializeField] private bool isHuntingFish = false;
    [SerializeField] private AudioClip slurp;
    [SerializeField] private AudioSource aud;
    [SerializeField] private AudioClip gasp;

    [Header("Leviathan Settings")]
    [SerializeField] private bool isLeviathan = false;
    private water waterScript;
    void Start()
    {
        flockManagerScript = FindFirstObjectByType<globalFlock>();
        objectRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        cameraDetection cameraScript = FindFirstObjectByType<cameraDetection>();
        cameraScript.RegisterFish(this);
        outline = GetComponentInChildren<Outline>();
        speed = Random.Range(minimumspeed, maximumspeed);
        sanityLevelEventManager = FindFirstObjectByType<sanityLevelEvents>();
        player = FindFirstObjectByType<Player>().gameObject;
        globalFlockScript = FindFirstObjectByType<globalFlock>();
        waterScript = FindAnyObjectByType<water>();
        aud = GetComponent<AudioSource>();
        if (outline != null)
            outline.enabled = false;
    }

    // Update is called once per frame


    void Update()
    {
        
        float distanceFromTrap;
        GameObject closestTrap = CheckDistanceFromTrap(out distanceFromTrap);

        if (distanceFromTrap < 10f)
        {
            transform.position = closestTrap.transform.position;
            return;
        }
        float distanceFromPlayer = CheckDistanceFromPlayer();
        if (isHuntingFish && distanceFromPlayer < huntDistance)
        {
            HuntPlayer();
        }
        CheckGround();
        if (sanityLevelEventManager.FishAllLookAtPlayer && !AllFishLookingAtPlayerTriggered)
        {
            Debug.Log("All fish looking at player triggered");
            AllFishLookingAtPlayerTriggered = true;
            StartCoroutine(AllFishLookAtPlayer());

        }
        float horizontalDistance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(globalFlock.tankCenter.x, globalFlock.tankCenter.z)
        );

        float verticalDistance = Mathf.Abs(
            transform.position.y - globalFlock.tankCenter.y
        );

        if (horizontalDistance >= globalFlock.HORIZONTALtankSize ||
            verticalDistance >= globalFlock.VERTICALtankSize)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
        if (turning)
        {
            Vector3 direction = globalFlock.tankCenter - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime);
            speed = Random.Range(minimumspeed, maximumspeed);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                ApplyRules();
            }
        }
        if (!lookingAtPlayer)
        {
            speedChangeTimer -= Time.deltaTime;
            if (speedChangeTimer <= 0f)
            {
                speed = Random.Range(minimumspeed, maximumspeed);
                speedChangeTimer = Random.Range(1f, 3f); // change every 1–3 seconds
            }
            transform.Translate(0, 0, Time.deltaTime * speed);
        }
        if (Vector3.Distance(player.transform.position, transform.position) < 7f && !isHuntingFish)
        {
            Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(fleeDirection);
            speed = fleeingSpeed;
        }
    }
    private void CheckGround()
    {
        if (Physics.Raycast(
            transform.position,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundLayer))
        {
            float groundDanger = 1f - (hit.distance / groundCheckDistance);

            Vector3 groundAvoidanceDirection =
                Quaternion.AngleAxis(-30f, transform.right) * transform.forward;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(groundAvoidanceDirection),
                rotationSpeed * groundDanger * Time.deltaTime
            );
        }
    }
    void ApplyRules()
    {
        GameObject[] gos = globalFlock.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;

        float gSpeed = 0.01f;
        if (isLeviathan)
        {
            goalpos = globalFlock.goalposLevi;
        }
        else
        {
            goalpos = globalFlock.goalpos;
        }

        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                float dist = Vector3.Distance(
                    go.transform.position,
                    transform.position
                );

                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 3.0f)
                    {
                        vavoid += transform.position - go.transform.position;
                    }
                    flock anotherFlock = go.GetComponent<flock>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize;
            vcentre += goalpos - transform.position;
            speed = gSpeed / groupSize;

            Vector3 direction = vcentre + vavoid - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
    
    IEnumerator AllFishLookAtPlayer()
    {
        float timer = 0f;
        lookingAtPlayer = true;
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        while (timer < 2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), rotationSpeed * Time.deltaTime*2);
            timer += Time.deltaTime;
            yield return null; // wait for next frame
        }
        lookingAtPlayer = false;
        ApplyRules();
    }
    public IEnumerator RunAway()
    {
        speed = fleeingSpeed;
        yield return new WaitForSeconds(fadeDuration);
        gameObject.SetActive(false);
    }
    public void PlayParticleEffect()
    {
        particleEffect.Play();
    }
    private GameObject CheckDistanceFromTrap(out float lowestDistanceFromTrap)
    {
        lowestDistanceFromTrap = Mathf.Infinity;
        GameObject closestTrap = null;

            foreach (GameObject game in flockManagerScript.fishTraps)
            {
                float distanceFromTrap = Vector3.Distance(
                    transform.position,
                    game.transform.position
                );

                if (distanceFromTrap < lowestDistanceFromTrap)
                {
                    lowestDistanceFromTrap = distanceFromTrap;
                    closestTrap = game;
                }
            }

        return closestTrap;
    }
    private float CheckDistanceFromPlayer()
    {
        float calculatedDistanceFromPlayer = Vector3.Distance(
            transform.position,
            player.transform.position
        );
        return calculatedDistanceFromPlayer;
    }
    private void HuntPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.deltaTime*2);
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer < 5f)
        {
            waterScript.oxygenLevel -= 10f;
            if (aud != null)
            {
                aud.clip = slurp;
                aud.Play();

                waterScript.audioSourceOneShots.clip = gasp;
                waterScript.audioSourceOneShots.Play();
            }
            StartCoroutine(HuntEnd());
        }
    }
    private IEnumerator HuntEnd()
    {
        isHuntingFish = false;
        yield return new WaitForSeconds(5f);
        isHuntingFish = true;
    }

}
