using System;
using UnityEngine;
    public enum ControlScheme
    {
        WASD,
        Arrows
    }

public class CharacterMovement : MonoBehaviour
{
    //public PlayerMoveData moveData;
    [SerializeField] public Collider2D col { get; private set; }
    Rigidbody2D rb;

    public ControlScheme inputType;

    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 70f;
    public float groundDeceleration = 70f;
    public float airDeceleration = 30f; // Less control in the air

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f; // Makes falling snappier
    public float lowJumpMultiplier = 2f; // Makes short taps snappier


    // Ground Check
    [Header("Ground Check")]
    //public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        switch (inputType)
        {
            case ControlScheme.WASD:
                WASDUpdate();
                break;
            case ControlScheme.Arrows:
                ArrowsUpdate();
                break;
        }

        if (direction != 0) lastDirection = new Vector2(direction, 0);
    }

    private void WASDUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpRequested = true;
            jumpHeld = true;
        }
        if(Input.GetKeyUp(KeyCode.W)) jumpHeld = false;

        if (Input.GetKeyDown(KeyCode.S)) // down
        {

        }
        direction = 0;
        if (Input.GetKey(KeyCode.D)) direction = 1;
        if (Input.GetKey(KeyCode.A)) direction = -1;
    }

    private void ArrowsUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpRequested = true;
            jumpHeld = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) jumpHeld = false;

        if (Input.GetKeyDown(KeyCode.DownArrow)) // down
        {

        }
        direction = 0;
        if (Input.GetKey(KeyCode.RightArrow)) direction = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) direction = -1;
    }
    private void FixedUpdate()
    {
        CheckGrounded();
        HandleHorizontalMovement(direction);
        HandleJump();
        HandleGravity();
    }

    public int direction;
    public Vector2 lastDirection { get; private set; }

    private void CheckGrounded()
    {
        Vector2 boxSize = new Vector2(col.bounds.size.x * 0.9f, groundCheckDistance);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(col.bounds.center, boxSize, 0f, Vector2.down,
            col.bounds.extents.y + groundCheckDistance);

        isGrounded = false;
        foreach (var hit in hits)
        {
            // Skip if it's our own collider
            if (hit.collider == col) continue;

            // Check for Ground component
            if (hit.collider.GetComponent<Ground>() != null)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void HandleHorizontalMovement( int dir) // put either 1 or -1
    {
        float targetSpeed = dir * maxSpeed;
        float currentSpeed = rb.linearVelocity.x;

        float adjustRate = 0f; // either accel or decel depending on situation
        if (Mathf.Abs(dir) > 0.01f)
        {
            adjustRate = acceleration;
        }
        else
        {
            adjustRate = isGrounded ? groundDeceleration : airDeceleration;
        }

        // Smoothly interpolate towards the target speed
        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, adjustRate * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newSpeed, rb.linearVelocity.y);
    }

    bool isGrounded;
    bool jumpRequested;
    bool jumpHeld;
    int jumpsLeft;

    private void HandleJump()
    {
        if (!jumpRequested) return;
        // Check if we can jump
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft = 1; // Now we have 1 air jump left
            jumpRequested = false;
            //Debug.Log("Grounded jump executed");
        }
        // Allow air jump if we have jumps left
        else if (jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsLeft--;
            jumpRequested = false;
            //Debug.Log($"Air jump executed. Jumps left: {jumpsLeft}");
        }
    }
    private void HandleGravity() // make character fall faster
    {
        if (rb.linearVelocity.y < 0) // if the character is falling
        {
            // make fall faster (-1 to account for unity's default gravity---we only wanna apply the difference between desired and default gravity)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jumpHeld) // low jump
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

    } 

    private void TryDrop() 
    {
        // only if floor is the kind that can be dropped down from (platforms)
    }

    private void OnDrawGizmosSelected()
    {
        if (col != null)
        {
            Gizmos.color = Color.red;
            Vector2 boxSize = new Vector2(col.bounds.size.x * 0.9f, groundCheckDistance);
            Gizmos.DrawWireCube(col.bounds.center + Vector3.down * (col.bounds.extents.y + groundCheckDistance / 2), boxSize);
        }
    }
}
