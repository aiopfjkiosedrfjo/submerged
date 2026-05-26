using UnityEngine;

public class Player : MonoBehaviour
{
    public water waterScript;
    public npcDetector npcDetector;
    public Transform orientation;
    public Transform REDROOMteleport;
    //sss
    
    // Ground Movement
    public Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float anchorForce = 15f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Checking when we're on the ground and keeping track of our ground check delay
        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Q) && npcDetector.anchorInteractable && isGrounded)
        {
            rb.linearVelocity = new Vector3(0, anchorForce, 0); // Jump interaction with NPC
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = REDROOMteleport.position;
        }


    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();

    }

    void MovePlayer()
    {

        Vector3 movementDirection = (orientation.forward * moveForward + orientation.right * moveHorizontal).normalized;
        Vector3 targetVelocity = movementDirection * MoveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        // If we aren't moving and are on the ground, stop velocity so we don't slide
        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        if (waterScript != null && !waterScript.inWater)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Initial burst for the jump
        }
        else if (waterScript != null && waterScript.inWater)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce / 3, rb.linearVelocity.z); // Reduced jump force in water
        }
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0) 
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } // Rising
        else if (rb.linearVelocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier  * Time.fixedDeltaTime;
        }
    }
}