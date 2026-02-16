using UnityEngine;

public class flock : MonoBehaviour
{
    float speed;
    public float minimumspeed = 2f;
    public float maximumspeed = 4f;
    float rotationSpeed = 4.0f;
    float neighbourDistance = 6.0f;
    public float debugSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(minimumspeed, maximumspeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,5) < 1)
        {
            ApplyRules();
        }
        if (Random.Range(0, 100) < 10)
        {
            speed = Random.Range(minimumspeed, maximumspeed);
            
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
        debugSpeed = Time.deltaTime * speed;
        Debug.Log(debugSpeed);
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
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime);
            }
        }
    }
}
