using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

    public enum HealthState
    {
        Normal,
        Downed
    }    
    public enum PlayerEquipment
    {
        None,
        Sword,
        Shield
    }

public class PlayerCharacter : MonoBehaviour
{
    CharacterMovement moveScript;
    public ColorType colorType;

    // need to be able to switch between sword and shield modes

    public PlayerEquipment currentEqipment;
    public PlayerEquipment previousEquipment;

    public HealthState healthState;

    public int playerMaxHP;
    public int playerCurrentHP;

    public ControlScheme inputType;

    public float respawnTime;

    private void Start()
    {
        moveScript = GetComponent<CharacterMovement>();
        if (moveScript == null) Debug.LogError("PlayerCharacter is missing a reference to the CharacterMovement script!");
        inputType = moveScript.inputType;

        playerMaxHP = 100;
        playerCurrentHP = playerMaxHP;
    }

    public bool ReadyToSwap;

    private void Update()
    {
        switch (currentEqipment)
        {
            case PlayerEquipment.None:
                NoEquipmentUpdate(); break;
            case PlayerEquipment.Sword:
                SwordUpdate(); break;
            case PlayerEquipment.Shield:
                ShieldUpdate(); break;
        }

        switch (healthState)
        {
            case HealthState.Normal:
                HealthyUpdate();
                break;
            case HealthState.Downed:
                DownedUpdate();
                break;
        }

    }
    private void HealthyUpdate()
    {
        // check inputs
        switch (inputType) // so that inputs are not recieved when player is downed
        {
            case ControlScheme.WASD:
                WASDUpdate();
                break;
            case ControlScheme.Arrows:
                ArrowsUpdate();
                break;
        }

        if (playerCurrentHP < 1)
        {
            OnDowned();
        }
    }

    private void WASDUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // attack
        {
            CanPerformAction = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)) // swap equipment
        {
            RequestSwapWeapon();
        }
    }
    private void ArrowsUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightShift)) // attack 
        {
            CanPerformAction = true;
        }
        if (Input.GetKeyDown(KeyCode.Slash)) // swap equipment
        {
            RequestSwapWeapon();
        }
    }

    bool CanPerformAction;

    private void NoEquipmentUpdate()
    {
        // wait to pick up a weapon


    }
    private void SwordUpdate()
    {
        if (CanPerformAction)
        {
            SwordAttack(moveScript.lastDirection);
            CanPerformAction = false;
        }
    }
    private void ShieldUpdate()
    {
        if (CanPerformAction)
        {
            ShieldStun(new Vector2(moveScript.direction, 0));
            CanPerformAction = false;
        }
    }

    private void RequestSwapWeapon()
    {
        ReadyToSwap = !ReadyToSwap;
        Debug.Log($"Ready to Swap is: {ReadyToSwap}");
    }
    public void SwapWeapons()
    {
        previousEquipment = currentEqipment;
        currentEqipment = PlayerEquipment.None;
    }

    private void SwordAttack(Vector2 dir)
    {
        // Box size - wider for attack range, slightly taller than player
        float attackRange = 3f;
        Vector2 smallBox = new Vector2(attackRange, moveScript.col.bounds.size.y * 1.2f);

        // Position the boxcast so it starts at player's edge and extends forward
        Vector2 originSmall = (Vector2)transform.position + (dir * (moveScript.col.bounds.extents.x + attackRange * 0.5f));

        RaycastHit2D[] smallHBox = Physics2D.BoxCastAll(originSmall, smallBox, 0, Vector2.zero, 0f);

        DrawBox(originSmall, smallBox, UnityEngine.Color.red, 1f);

        foreach (var hit in smallHBox)
        {
            if (hit.collider == moveScript.col) continue;

            if (dir.x > -1)
            {
                VFXManager.instance.playSlash(gameObject.transform.position, 45);
            }
            else
            {
                VFXManager.instance.playSlash(gameObject.transform.position, -90);
            }

            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                Enemy enemyHP = hit.collider.gameObject.GetComponent<Enemy>();
                if (enemyHP.stunDuration > 0)
                {
                    enemyHP.TakeDamage(20);
                }
                else
                {
                    enemyHP.TakeDamage(35);
                }
            }
        }
    }
    private void ShieldStun(Vector2 dir)
    {
        // Box size - wider for attack range, slightly taller than player
        float attackRange = 3f;
        Vector2 smallBox = new Vector2(attackRange, moveScript.col.bounds.size.y * 1.2f);

        // Position the boxcast so it starts at player's edge and extends forward
        Vector2 originSmall = (Vector2)transform.position + (dir * (moveScript.col.bounds.extents.x + attackRange * 0.5f));

        RaycastHit2D[] smallHBox = Physics2D.BoxCastAll(originSmall, smallBox, 0, Vector2.zero, 0f);

        DrawBox(originSmall, smallBox, UnityEngine.Color.red, 1f);

        foreach (var hit in smallHBox)
        {
            if (hit.collider == moveScript.col) continue;

            if (dir.x > -1)
            {
                //VFXManager.instance.playSlash(gameObject.transform.position, 45);
            }
            else
            {
                //VFXManager.instance.playSlash(gameObject.transform.position, -90);
            }

            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                Enemy enemyHP = hit.collider.gameObject.GetComponent<Enemy>();
                enemyHP.stun(2);
            }
        }
    }

    float RespawnTime => respawnTime;
    private void DownedUpdate()
    {
        respawnTime -= Time.deltaTime;
        if (respawnTime < 0)
        {
            respawnTime = 0;
            // switch to healthy
            OnRevived();
        }
    }

    public void OnDowned()
    {
        respawnTime = 15;
        healthState = HealthState.Downed;
    }
    public void OnRevived()
    {
        healthState = HealthState.Normal;
        playerCurrentHP = playerMaxHP;
    }

    public void TakeDamage(int damage)
    {
        playerCurrentHP -= damage;
    }

    private void DrawBox(Vector2 center, Vector2 size, UnityEngine.Color color, float duration = 1f)
    {
        Vector2 halfSize = size * 0.5f;

        Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
        Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);
        Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
        Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);

        Debug.DrawLine(bottomLeft, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, topRight, color, duration);
        Debug.DrawLine(topRight, topLeft, color, duration);
        Debug.DrawLine(topLeft, bottomLeft, color, duration);
    }
}
