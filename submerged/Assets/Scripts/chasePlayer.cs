using UnityEngine;

public class chasePlayer : MonoBehaviour
{
    public float speed = 10f;
    private Player player;
    [SerializeField] private AudioSource aud;
    [SerializeField] private AudioSource aud2;
    private bool hasSpawnedMask = false;
    public GameObject breathTP;
    private void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }
    private void Update()
    {
        if (player != null) 
        {
            transform.LookAt(player.gameObject.transform);
            transform.position = Vector3.MoveTowards(transform.position, player.gameObject.transform.position, speed * Time.deltaTime);
        }
        float distanceFromPlayer = Vector3.Distance(transform.position, player.gameObject.transform.position);
        if (distanceFromPlayer < 5f)
        {
            EndChaseSequence();
        }
    }
    private void EndChaseSequence()
    {
        aud.Stop();
        if (!aud2.isPlaying)
            aud2.Play();
        transform.position = breathTP.transform.position;
        if (!hasSpawnedMask)
        {
            triggerManager.instance.enableMask();
            hasSpawnedMask = true;
        }
    }
}
