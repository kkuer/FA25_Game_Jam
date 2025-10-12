using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BishopAI : MonoBehaviour
{
    public Transform player; 

    public float radius = 5f;
    public float rayDistance = 5f;

    public LayerMask ignore;

    public float moveSpeed = 2f;
    public float patrolRange = 3f;
    public float jumpHorizontalSpeed = 4f;
    public float jumpVerticalSpeed = 7f;

    private Rigidbody2D rb;
    private Vector2 startPos;
    private int dir = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) { Patrol(); return; }

        bool inRadius = Vector2.Distance(transform.position, player.position) <= radius;

        bool onDiagonal = false;
        Vector2 origin = transform.position;
        Vector2[] dirs = new Vector2[]
        {
            new Vector2( 1f,  1f).normalized,
            new Vector2(-1f,  1f).normalized,
            new Vector2( 1f, -1f).normalized,
            new Vector2(-1f, -1f).normalized
        };

        for (int i = 0; i < dirs.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, dirs[i], rayDistance, ignore);
            if (hit.collider != null)
            {
                //if (hit.collider.gameObject == gameObject) { continue; }

                if (hit.collider.transform == player)
                {
                    Debug.Log("Jump!");
                    onDiagonal = true;
                    break;
                }

            }
        }
        //RadiusRaycastCheck
        if (inRadius && onDiagonal)
        {
            JumpTowardAndAttack();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (Mathf.Abs(transform.position.x - startPos.x) >= patrolRange)
            dir *= -1;

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    private void JumpTowardAndAttack()
    {
        float signX = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(signX * jumpHorizontalSpeed, jumpVerticalSpeed);
        Debug.Log($"{name} jumps toward and attacks {player.name}");
    }
}
