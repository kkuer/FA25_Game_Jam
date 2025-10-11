using UnityEngine;

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

    private void Start()
    {
        moveScript = GetComponent<CharacterMovement>();
    }

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
    }
    private void NoEquipmentUpdate()
    {
        // move slightly faster to pick up weapon
    }
    private void SwordUpdate()
    {

    }
    private void ShieldUpdate()
    {

    }

    private void SwapWeapon()
    {
        // animation tree bullshit
        // 

        Debug.Log("Weapon has been dropped");
        currentEqipment = PlayerEquipment.None;
    }
}
