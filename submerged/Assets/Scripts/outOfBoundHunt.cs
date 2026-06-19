using UnityEngine;

public class outOfBoundHunt : MonoBehaviour
{
    public GameObject playerObj;
    public Transform playerPos;
    public bool chasingPlayer;
    public float speed = 10f;
    public float rotationSpeed = 10f;
    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerObj.transform;

    }
    void Update()
    {
        if (chasingPlayer)
        {
            Vector3 dir = (playerPos.position - transform.position).normalized;
            Quaternion lookrot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookrot, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
