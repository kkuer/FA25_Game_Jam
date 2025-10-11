using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //public PlayerMoveData moveData;
    Rigidbody2D rb;

    public enum ControlScheme
    {
        WASD,
        Arrows
    }
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

    bool grounded;


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
    }

    private void FixedUpdate()
    {

    }

    private void WASDUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpRequested = true;
            if (grounded)
            {
                jumpsLeft = 2;
            }
            jumpHeld = true;
        }
        if(Input.GetKeyUp(KeyCode.W) && jumpHeld) jumpHeld = false;

        if (Input.GetKeyDown(KeyCode.S)) // down
        {

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
        }
    }

    private void ArrowsUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpRequested = true;
            if (grounded)
            {
                jumpsLeft = 2;
            }
            jumpHeld = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && jumpHeld) jumpHeld = false;

        if (Input.GetKeyDown(KeyCode.DownArrow)) // down
        {

        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
        }
    }

    private void HandleHorizontalMovement( int dir) // put either 1 or -1
    {
        float targetSpeed = dir * maxSpeed;
        float currentSpeed = rb.linearVelocity.x;

    }

    bool jumpRequested;
    bool jumpHeld;
    int jumpsLeft;

    private void HandleJump()
    {
        if (!jumpRequested) return;
        if (jumpsLeft < 1) return;

        rb.linearVelocity = new Vector2( rb.linearVelocity.x, jumpForce); // directly sets y velocity
        --jumpsLeft;

        jumpRequested = false;
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
}
