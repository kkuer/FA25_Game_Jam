using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnightAI : MonoBehaviour
{
    public Transform player;
    public float radius = 6f;
    public float rayDistance = 8f;
    public LayerMask detectMask;
    public float dashSpeed = 10f;
    public float dashTime = 0.3f;
    public int dashDamage = 15;
    public float attackCooldown = 1.2f;
    public float moveSpeed = 2f;
    public float patrolRange = 3f;
    private Rigidbody2D rb;
    private Vector2 startPos;
    private int dir = 1;
    private bool isDashing = false;
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
        if (dist <= radius && !isDashing)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, rayDistance, detectMask);

            if (hit.collider != null && hit.collider.transform == player)
            {
                StartCoroutine(DashAttack(dirToPlayer));
                return;
            }
        }

        if (!isDashing)
            Patrol();
    }

    private void Patrol()
    {
        if (Mathf.Abs(transform.position.x - startPos.x) >= patrolRange)
            dir *= -1;

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    private System.Collections.IEnumerator DashAttack(Vector2 dashDir)
    {
        if (Time.time - lastAttackTime < attackCooldown) yield break;

        lastAttackTime = Time.time;
        isDashing = true;

        float timer = 0f;
        while (timer < dashTime)
        {
            rb.linearVelocity = dashDir * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        if (Vector2.Distance(transform.position, player.position) < 1.5f)
        {
            DealDamageToPlayer(dashDamage);
            Debug.Log($"{name} dashed into {player.name} for {dashDamage}");
        }
    }

    private void DealDamageToPlayer(int dmg)
    {
        Debug.Log($"[Damage] {player.name} takes {dmg}");
    }
}
