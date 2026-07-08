using UnityEngine;

public class doorTeleport : MonoBehaviour
{
    private Vector3 redRoomTeleport;
    [SerializeField]private Player player;
    private void Awake()
    {
        Player player = FindFirstObjectByType<Player>();
        Vector3 redRoomTeleport = player.REDROOMteleport.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.rb.linearVelocity = Vector3.zero;
            player.rb.position = redRoomTeleport;
        }
    }
}
