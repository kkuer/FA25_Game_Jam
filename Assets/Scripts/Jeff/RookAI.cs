using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RookAI : MonoBehaviour
{
    public Transform player;
    public float radius1 = 8f;
    public float radius2 = 2.5f;
    public LayerMask groundMask;
    public float moveSpeed = 2.5f;
    public float patrolRange = 3f;
    public float jumpVerticalSpeed = 8f;
    public float jumpFreezeTime = 0.15f;
    public float aoeRadius = 2.2f;
    public int aoeDamage = 20;
    public int directDamage = 12;
    public float attackCooldown = 1.2f;
    private Rigidbody2D rb;
    private Vector2 startPos;
    private int patrolDir = 1;
    private bool seenInRadius1 = false;
    private bool isJumping = false;
    private float lastAttackTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null)
        {
            Patrol();
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);
        bool inR1 = dist <= radius1;
        bool inR2 = dist <= radius2;

        if (inR1) seenInRadius1 = true;

        if (isJumping)
        {
            if (rb.linearVelocity.y <= 0f && IsGrounded())
            {
                isJumping = false;
                DoAOEDamage();
            }
            return;
        }

        if (inR2)
        {
            if (seenInRadius1)
            {
                StartCoroutine(JumpAttackRoutine());
            }
            else
            {
                TryDirectDamage();
            }
            return;
        }

        if (inR1)
        {
            Chase();
        }
        else
        {
            seenInRadius1 = false;
            Patrol();
        }
    }


    private void Patrol()
    {
        if (Mathf.Abs(transform.position.x - startPos.x) >= patrolRange)
            patrolDir *= -1;

        rb.linearVelocity = new Vector2(patrolDir * moveSpeed, rb.linearVelocity.y);
    }

    private void Chase()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    private System.Collections.IEnumerator JumpAttackRoutine()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        yield return new WaitForSeconds(jumpFreezeTime);

        isJumping = true;
        rb.linearVelocity = new Vector2(0f, jumpVerticalSpeed);
        seenInRadius1 = false;
    }

    private void TryDirectDamage()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        DealDamageToPlayer(directDamage);
        Debug.Log($"{name} direct-attacked {player.name} for {directDamage}");
    }

    private void DoAOEDamage()
    {
        if (Vector2.Distance(transform.position, player.position) <= aoeRadius)
        {
            DealDamageToPlayer(aoeDamage);
            Debug.Log($"{name} landed AOE on {player.name} for {aoeDamage}");
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = transform.position;
        float checkDist = 0.2f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, checkDist, groundMask);
        return hit.collider != null;
    }
    private void DealDamageToPlayer(int dmg)
    {
        var hp = player.GetComponent(typeof(MonoBehaviour)) as MonoBehaviour;
        Debug.Log($"[Damage] {player.name} takes {dmg}");
    }
}
