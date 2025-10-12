using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enemyType enemy = new enemyType();
    //HP
    public int hp = 100;
    public int maxHp = 100;
    //DMG
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} Defeated!");
        GameManager.instance.activeEnemies.Remove(gameObject);
        Destroy(gameObject);
    }

    private void Start()
    {
        hp = maxHp;
    }
}
