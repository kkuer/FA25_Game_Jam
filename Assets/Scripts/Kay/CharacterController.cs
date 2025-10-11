using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // fucking rigidbody stuff

    // need to be able to switch between sword and shield modes
    public enum PlayerEquipment
    {
        None,
        Sword,
        Shield
    }
    public PlayerEquipment currentEqipment;

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

    }
    private void SwordUpdate()
    {

    }
    private void ShieldUpdate() 
    {
    
    }
}
