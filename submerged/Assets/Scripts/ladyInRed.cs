using UnityEngine;

public class ladyInRed : MonoBehaviour
{
    [SerializeField] private Player player;
    void Update()
    {
        if (player != null)
        {
            Vector3 targetPos = new Vector3(player.gameObject.transform.position.x, transform.position.y, player.gameObject.transform.position.z );
            transform.LookAt(targetPos);
        }

    }
    public void PassPlayer(Player target)
    {
        player = target;
    }
}
