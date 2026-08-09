using System.Collections;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraDetection cameraScript = FindFirstObjectByType<cameraDetection>();
        cameraScript.RegisterFish(this);
        outline = GetComponent<Outline>();
        speed = Random.Range(minimumspeed, maximumspeed);
        sanityLevelEventManager = FindFirstObjectByType<sanityLevelEvents>();
        player = FindFirstObjectByType<Player>().gameObject;
        globalFlockScript = FindFirstObjectByType<globalFlock>();
    }

    // Update is called once per frame


    void Update()
    {
        if (sanityLevelEventManager.FishAllLookAtPlayer && !AllFishLookingAtPlayerTriggered)
        {
            Debug.Log("All fish looking at player triggered");
            AllFishLookingAtPlayerTriggered = true;
            StartCoroutine(AllFishLookAtPlayer());

        }
        if (Vector3.Distance(transform.position, globalFlock.tankCenter) >= globalFlock.HORIZONTALtankSize)
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
        if (Vector3.Distance(player.transform.position, transform.position) < 7f)
        {
            Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(fleeDirection);
            speed = fleeingSpeed;
            
        }
    }
    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.allFish;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        Vector3 goalpos = globalFlock.goalpos;
        float dist;
        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 3.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    flock anotherFlock = go.GetComponent<flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalpos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = vcentre + vavoid - transform.position;
            if (direction != globalFlock.tankCenter - transform.position)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime);
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
}
