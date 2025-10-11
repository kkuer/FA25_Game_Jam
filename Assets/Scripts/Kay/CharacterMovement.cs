using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public PlayerMoveData moveData;

    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, moveData.jumpForce) * rb.mass / Time.deltaTime);

        // no Forcemode2D.VelocityChange, so just simulate it by using this equation: force * rigidbody2D.mass / Time.fixedDeltaTime
        // purpose of VelocityChange is for more responsive, less weighty movement (functions like impulse would, it just ignores mass)
    }
}
