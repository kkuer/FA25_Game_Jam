using Unity.VisualScripting;
using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class PlayerCharacter : MonoBehaviour
{
    CharacterMovement moveScript;
    public ColorType colorType;

    // need to be able to switch between sword and shield modes
    public enum PlayerEquipment
    {
        None,
        Sword,
        Shield
    }
    public PlayerEquipment currentEqipment;
    public PlayerEquipment previousEquipment;

    public ControlScheme inputType;

    private void Start()
    {
        moveScript = GetComponent<CharacterMovement>();
        if (moveScript == null) Debug.LogError("PlayerCharacter is missing a reference to the CharacterMovement script!");
        inputType = moveScript.inputType;
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

        // check inputs
        switch (inputType)
        {
            case ControlScheme.WASD:
                WASDUpdate();
                break;
            case ControlScheme.Arrows:
                ArrowsUpdate();
                break;
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
            SwordAttack(new Vector2(moveScript.direction, 0));
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
        Vector2 smallBox = new Vector2(1.5f, 1.5f);
        Vector2 largeBox = new Vector2(2, 3); // origin can be center 

        Vector2 originSmall = Vector2.zero;
        Vector2 originLarge = Vector2.zero;

        float originHeight = (moveScript.col.bounds.size.y / 4) * 3;

        originSmall = new Vector2(moveScript.col.bounds.center.x, originHeight);

        RaycastHit2D[] smallHBox = Physics2D.BoxCastAll(originSmall, smallBox, 0, dir);
        RaycastHit2D[] largeHBox = Physics2D.BoxCastAll(originLarge, largeBox, 0, dir);

        foreach (var hit in smallHBox)
        {
            // Skip if it's our own collider
            if (hit.collider == moveScript.col) continue;

            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                // do like... 2.5 damage
                Enemy enemyHP = hit.collider.gameObject.GetComponent<Enemy>();
                enemyHP.TakeDamage(3);
            }
        }
        foreach (var hit in largeHBox)
        {
            // Skip if it's our own collider
            if (hit.collider == moveScript.col) continue;

            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                // 5 damage
                Enemy enemyHP = hit.collider.gameObject.GetComponent<Enemy>();
                enemyHP.TakeDamage(5);
            }
        }
    }
    private void ShieldStun(Vector2 dir)
    {
        Vector2 smallBox = new Vector2(1.5f, 1.5f);
        Vector2 largeBox = new Vector2(2, 3); // origin can be center 

        Vector2 originSmall = Vector2.zero;
        Vector2 originLarge = Vector2.zero;

        float originHeight = (moveScript.col.bounds.size.y / 4) * 3;

        originSmall = new Vector2(moveScript.col.bounds.center.x, originHeight);

        RaycastHit2D[] smallHBox = Physics2D.BoxCastAll(originSmall, smallBox, 0, dir);
        RaycastHit2D[] largeHBox = Physics2D.BoxCastAll(originLarge, largeBox, 0, dir);

        foreach (var hit in smallHBox)
        {
            // Skip if it's our own collider
            if (hit.collider == moveScript.col) continue;

            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                // stun for additional 1.5 seconds
            }
        }
        foreach (var hit in largeHBox)
        {
            // Skip if it's our own collider
            if (hit.collider == moveScript.col) continue;

            if (hit.collider.gameObject.GetComponent<Enemy>() != null)
            {
                // stun enemy for 2 seconds
            }
        }
    }
}
