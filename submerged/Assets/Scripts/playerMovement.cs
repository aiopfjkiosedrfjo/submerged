using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public water waterScript;
    public npcDetector npcDetector;
    public Transform orientation;
    public Transform REDROOMteleport;
    public Transform boatTeleport;
    public InputActionReference Jetpack;
    //sss
    
    // Ground Movement
    public Rigidbody rb;
    public bool isRidingBoat = false;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;
    private bool holdingJetpack = false;
    public float jetPackAmount = 5f;
    public float jetPackAmountOriginal = 5f;
    public float jetPackForce = 3f;

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
    void OnEnable()
    {
        Jetpack.action.started += OnJetpackStart;
        Jetpack.action.canceled += OnJetpackEnd;
    }

    void OnDisable()
    {
        Jetpack.action.started -= OnJetpackStart;
        Jetpack.action.canceled -= OnJetpackEnd;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isRidingBoat = !isRidingBoat;

        }

        if (isGrounded)
        {
            jetPackAmount = jetPackAmountOriginal;
        }
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        if (holdingJetpack && jetPackAmount > 0f && !isGrounded && waterScript.inWater)
        {
            rb.AddForce(new Vector3(0, jetPackForce, 0), ForceMode.VelocityChange);
            jetPackAmount -= 1f;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = REDROOMteleport.position;
        }


    }
    void OnJetpackStart(InputAction.CallbackContext context)
    {
        holdingJetpack = true;
    }

    void OnJetpackEnd(InputAction.CallbackContext context)
    {
        holdingJetpack = false;
    }


    void FixedUpdate()
    {
        if (isRidingBoat)
        {
            rb.MovePosition(boatTeleport.position);  
            rb.isKinematic = true; // Disable physics while riding the boat
            return;
        }
        MovePlayer();
        ApplyJumpPhysics();

    }

    void MovePlayer()
    {
        rb.isKinematic = false; // Ensure physics is enabled when not riding the boat
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
    public void Die()
    {
        rb.linearVelocity = Vector3.zero;
        rb.position = new Vector3(-2,6,16);
    }
}