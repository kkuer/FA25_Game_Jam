using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class RookAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float radius1 = 8f;
    public float radius2 = 2.5f;

    [Header("Movement Settings")]
    public LayerMask groundMask;
    public float moveSpeed = 2.5f;
    public float patrolRange = 3f;

    [Header("Attack Settings")]
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
    private bool hasTriggeredJumpAnim = false;
    private float lastAttackTime = -999f;

    private PlayerCharacter currentTarget;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 🔹 Update walking animation
        if (anim != null)
        {
            bool movingHorizontally = Mathf.Abs(rb.linearVelocity.x) > 0.05f;
            anim.SetBool("isWalk", movingHorizontally);
        }

        // If no players exist, patrol
        if (MasterCharacterManager.instance.players.Count == 0)
        {
            Patrol();
            return;
        }

        // Find closest player
        PlayerCharacter closestPlayer = GetClosestPlayer();
        if (closestPlayer == null)
        {
            Patrol();
            return;
        }

        currentTarget = closestPlayer;
        float dist = Vector2.Distance(transform.position, currentTarget.transform.position);
        bool inR1 = dist <= radius1;
        bool inR2 = dist <= radius2;

        if (inR1) seenInRadius1 = true;

        // Handle jump landing
        if (isJumping)
        {
            if (rb.linearVelocity.y <= 0f && IsGrounded())
            {
                isJumping = false;
                hasTriggeredJumpAnim = false;

                if (anim != null)
                {
                    anim.SetBool("isJump", false);
                    anim.ResetTrigger("GoJump");
                }

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

        // 🔹 Reset attack trigger when animation finishes
        if (anim != null)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("Attack") && state.normalizedTime >= 1f)
            {
                anim.ResetTrigger("GoAtk");
                anim.SetBool("isAttack", false);
            }

            if (state.IsName("Jump") && state.normalizedTime >= 1f)
            {
                anim.ResetTrigger("GoJump");
            }
        }
    }

    private PlayerCharacter GetClosestPlayer()
    {
        if (MasterCharacterManager.instance.players.Count == 0) return null;

        PlayerCharacter closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (PlayerCharacter player in MasterCharacterManager.instance.players)
        {
            if (player == null) continue;
            float distance = Vector2.Distance(transform.position, player.gameObject.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = player;
            }
        }

        return closest;
    }

    private void Patrol()
    {
        if (Mathf.Abs(transform.position.x - startPos.x) >= patrolRange)
            patrolDir *= -1;

        rb.linearVelocity = new Vector2(patrolDir * moveSpeed, rb.linearVelocity.y);
    }

    private void Chase()
    {
        if (currentTarget == null) return;

        float dir = Mathf.Sign(currentTarget.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    private System.Collections.IEnumerator JumpAttackRoutine()
    {
        if (isJumping || hasTriggeredJumpAnim) yield break; // Prevent double triggering

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        yield return new WaitForSeconds(jumpFreezeTime);

        isJumping = true;
        hasTriggeredJumpAnim = true;

        // 🔹 Trigger jump animation once
        if (anim != null)
        {
            anim.SetTrigger("GoJump");
            anim.SetBool("isJump", true);
        }

        rb.linearVelocity = new Vector2(0f, jumpVerticalSpeed);
        seenInRadius1 = false;
    }

    private void TryDirectDamage()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        if (anim != null)
        {
            anim.SetTrigger("GoAtk");
            anim.SetBool("isAttack", true);
        }

        DealDamageToPlayer(directDamage);
    }

    private void DoAOEDamage()
    {
        if (currentTarget != null && Vector2.Distance(transform.position, currentTarget.gameObject.transform.position) <= aoeRadius)
        {
            if (anim != null)
            {
                anim.SetTrigger("GoAtk");
                anim.SetBool("isAttack", true);
            }

            DealDamageToPlayer(aoeDamage);

            ShakeManager.instance.shakeCam(4f, 0.25f, 0.2f);
            VFXManager.instance.playStomp(gameObject.transform.position);
            SoundManager.instance.playSFX(SoundManager.instance.rookJump);
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = transform.position;
        float checkDist = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, checkDist, groundMask);
        return hit.collider != null;
    }

    private void DealDamageToPlayer(int dmg)
    {
        if (currentTarget != null)
        {
            currentTarget.TakeDamage(dmg);
            VFXManager.instance.playHit(currentTarget.transform.position, true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius2);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
