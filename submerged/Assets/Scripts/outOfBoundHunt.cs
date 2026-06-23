using UnityEngine;

public class outOfBoundHunt : MonoBehaviour
{
    private GameObject playerObj;
    private Transform playerPos;
    [SerializeField]private AudioClip screech;
    public AudioSource aud;
    public bool chasingPlayer;
    private bool reachedPlayer = false;
    public float speed = 10f;
    public float rotationSpeed = 10f;
    private bool hasPlayedScreech;
    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerObj.transform;

    }
    void Update()
    {
        if (canChasePlayer())
        {
            if (Vector3.Distance(transform.position, playerObj.transform.position) <= 200f)
            {
                if (!hasPlayedScreech)
                {
                    aud.PlayOneShot(screech);
                    hasPlayedScreech = true;
                }
            }
            else
            {
                hasPlayedScreech = false;
            }
            Vector3 dir = (playerPos.position - transform.position).normalized;
            Quaternion lookrot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookrot, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            Debug.Log("Implement Killing / Jumpscare logic");
            Destroy(gameObject);
            //Play jumpscare and kill player
        }
    }
    public bool canChasePlayer()
    {
        if (Vector3.Distance(transform.position, playerObj.transform.position) <= 40f) return false;
            
        return true;
    }
}
