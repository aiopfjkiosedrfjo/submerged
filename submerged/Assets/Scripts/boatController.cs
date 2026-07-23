using UnityEngine;

public class boatController : MonoBehaviour
{
    public float boatSpeed = 5f;
    public float turnSpeed = 1f;
    public Rigidbody rb;
    [SerializeField] private MeshCollider boatCollider;
    public Player player;
    public void FixedUpdate()
    {
        if (player.isRidingBoat)
        {
            MoveBoat();
        }
        else
        {
            boatCollider.enabled = true;
            rb.isKinematic = true;
        }

    }
    public void MoveBoat()
    {
        rb.isKinematic = false;
        boatCollider.enabled = false;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontalInput) != 0)
        {
            rb.angularVelocity = new Vector3(0, horizontalInput * turnSpeed, 0);
        }
        if (Mathf.Abs(verticalInput) != 0)
        {
            rb.linearVelocity = transform.forward * verticalInput * boatSpeed;
        }
    }
}
