using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enemyType enemy = new enemyType();

    public int hp = 100;
    public int maxHp = 100;

    public float stunDuration;

    private Rigidbody2D rb;

    private void Start()
    {
        hp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        stunDuration = 0;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        VFXManager.instance.playHit(transform.position, false);
        ShakeManager.instance.shakeCam(3f, 0.1f, 0.2f);

        if (hp <= 0)
        {
            die();
        }
    }

    private void Update()
    {
        if (stunDuration > 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            stunDuration -= Time.deltaTime;
            if (stunDuration <= 0)
            {
                stunDuration = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }   
    }

    public void stun(float duration)
    {
        stunDuration += duration;
        VFXManager.instance.playStun(transform.position, stunDuration);
    }

    private void die()
    {
        VFXManager.instance.playDeath(transform.position);
        ShakeManager.instance.shakeCam(5f, 0.1f, 0.2f);
        //play sound
        GameManager.instance.activeEnemies.Remove(gameObject);
        Destroy(gameObject);
    }
}
