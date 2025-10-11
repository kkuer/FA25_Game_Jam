using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 3f;
    private Vector2 startPos;
    private int direction = 1;
    public int maxHP = 5;
    private int currentHP;
    private void Start()
    {
        startPos = transform.position;
        currentHP = maxHP;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
        
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1; 
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} Defeated!");
        Destroy(gameObject);
    }
}
