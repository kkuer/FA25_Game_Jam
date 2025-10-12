using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public enemyType enemy = new enemyType();
    //HP
    public int currentHP = 100;
    public int maxHP = 100;
    //DMG
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! Remaining HP: {currentHP}");

        if (currentHP <= 0)
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
        currentHP = maxHP;
    }
}
