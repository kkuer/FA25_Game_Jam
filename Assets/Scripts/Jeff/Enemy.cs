using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public enemyType enemy = new enemyType();

    public int hp = 100;
    public int maxHp = 100;

    private Rigidbody2D rb;

    private void Start()
    {
        hp = maxHp;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            die();
        }
    }

    public void stun(float duration)
    {
        StartCoroutine(stunlock(duration));
    }

    private void die()
    {
        GameManager.instance.activeEnemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public IEnumerator stunlock(float duration)
    {
        //stun logic
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation; 

        yield return new WaitForSeconds(duration);
        //return to normal
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
