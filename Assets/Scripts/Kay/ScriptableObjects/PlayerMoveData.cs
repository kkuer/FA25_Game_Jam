using UnityEngine;

[CreateAssetMenu( menuName = "Player Data/PlayerMoveData")]
public class PlayerMoveData : ScriptableObject
{
    public float jumpForce;
    public float downwardForce; // height at which downward force will be applied 

    public float horizontalForce;
}
