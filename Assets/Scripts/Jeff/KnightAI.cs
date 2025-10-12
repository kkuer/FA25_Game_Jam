using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnightAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float radius = 6f;
    public float rayDistance = 8f;
    public LayerMask detectMask;

    [Header("Attack Settings")]
    public float dashSpeed = 10f;
    public float dashTime = 0.3f;
    public int dashDamage = 15;
    public float attackCooldown = 1.2f;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float patrolRange = 3f;

    private Rigidbody2D rb;
    private Vector2 startPos;

    private int dir = 1;
    private bool isDashing = false;
    private float lastAttackTime = -999f;
    private PlayerCharacter currentTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update()
    {
        // If no players exist, just patrol
        if (MasterCharacterManager.instance.players.Count == 0)
        {
            Patrol();
            return;
        }

        // Find potential targets
        currentTarget = GetBestTarget();
        if (currentTarget == null)
        {
            Patrol();
            return;
        }

        float dist = Vector2.Distance(transform.position, currentTarget.gameObject.transform.position);
        if (dist <= radius && !isDashing)
        {
            Vector2 dirToPlayer = (currentTarget.gameObject.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, rayDistance, detectMask);

            if (hit.collider != null)
            {
                // Debug the hit object
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

                if (IsPlayer(hit.collider.transform))
                {
                    StartCoroutine(DashAttack(dirToPlayer));
                    return;
                }
                else
                {
                    Debug.Log($"Hit object {hit.collider.gameObject.name} is not a player");
                }
            }
            else
            {
                Debug.Log("Raycast didn't hit anything");
            }
        }

        if (!isDashing)
            Patrol();
    }

    private PlayerCharacter GetBestTarget()
    {
        if (MasterCharacterManager.instance.players.Count == 0) return null;

        PlayerCharacter bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (PlayerCharacter player in MasterCharacterManager.instance.players)
        {
            if (player == null) continue;

            float distance = Vector2.Distance(transform.position, player.gameObject.transform.position);

            // Check if player is within detection radius
            if (distance <= radius)
            {
                Vector2 dirToPlayer = (player.gameObject.transform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, rayDistance, detectMask);

                // Draw debug line
                Debug.DrawLine(transform.position, player.gameObject.transform.position, Color.red);

                if (hit.collider != null && IsPlayer(hit.collider.transform) && distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTarget = player;
                }
            }
        }
        return bestTarget;
    }

    private bool IsPlayer(Transform targetTransform)
    {
        foreach (PlayerCharacter player in MasterCharacterManager.instance.players)
        {
            if (player != null && player.gameObject != null)
            {
                // Check if the hit transform is the player's transform OR any of its children
                if (targetTransform == player.transform || targetTransform.IsChildOf(player.transform))
                {
                    return true;
                }
            }
        }
        return false;
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

        // Check damage against all players in range after dash
        CheckDashDamage();
    }

    private void CheckDashDamage()
    {
        foreach (PlayerCharacter player in MasterCharacterManager.instance.players)
        {
            if (player == null) continue;

            if (Vector2.Distance(transform.position, player.gameObject.transform.position) < 1.5f)
            {
                DealDamageToPlayer(player, dashDamage);
            }
        }
    }

    private void DealDamageToPlayer(PlayerCharacter targetPlayer, int dmg)
    {
        targetPlayer.TakeDamage(dmg);
        ShakeManager.instance.shakeCam(4f, 0.25f, 0.2f);
        VFXManager.instance.playHit(currentTarget.transform.position, true);
    }

    // Optional: Visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentTarget.gameObject.transform.position);
        }
    }
}
