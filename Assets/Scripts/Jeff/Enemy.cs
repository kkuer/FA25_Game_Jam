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
    }

    private void die()
    {
        GameManager.instance.activeEnemies.Remove(gameObject);
        Destroy(gameObject);
    }
}
