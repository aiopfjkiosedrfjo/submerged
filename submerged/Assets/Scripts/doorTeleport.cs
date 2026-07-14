using UnityEngine;

public class doorTeleport : MonoBehaviour
{
    private Vector3 redRoomTeleport;
    public Player player;
    public void SetPlayer(Player target)
    {
        player = target;
        redRoomTeleport = player.REDROOMteleport.position;
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
