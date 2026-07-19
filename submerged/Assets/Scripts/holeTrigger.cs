using UnityEngine;

public class holeTrigger : MonoBehaviour
{
    [SerializeField] private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.rb.position = player.boatTeleport.position;
            player.rb.linearVelocity = Vector3.zero;
        }
    }
}
