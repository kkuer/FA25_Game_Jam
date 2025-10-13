using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    public PlayerCharacter character;
    public CharacterMovement playerMovement;
    public SpriteRenderer spriteRenderer;
    public string lastAnimation;
    public bool isRomena;
    public bool shouldFlipX;
    public float minDetectedInput = 0.1f;
    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        character = GetComponent<PlayerCharacter>();
        playerMovement = GetComponent<CharacterMovement>();
        spriteRenderer.GetComponentInChildren<SpriteRenderer>();
    }

    // Romena/Black Pawn animation names
    string swordIdleR = "Romena_IdleL_S";
    string swordWalkR = "Romena_WalkL_S";
    string swordJumpR = "Romena_StartJumpL_S";
    string swordFallR = "Romena_FallL_S";
    //string swordSustainR = ;
    string swordAttackR = "Romena_Atk_S";

    string shieldIdleR = "Romena_IdleL_D";
    string shieldWalkR = "Romena_WalkL_D";
    string shieldJumpR = "Romena_StartJumpL_D";
    //string shieldFallR = ;
    //string shieldSustainR = ;
    string shieldAttackR = "Romena_Atk_D";

    // function for animations
    public void PlayAnimationByName(string animationToPlay)
    {
        lastAnimation = animationToPlay;
        playerAnimator.CrossFade(animationToPlay, 0.1f);

    }
    private void Update()
    {
        SetFacingDirection();
        ChooseAnimationToPlay();
    }
    public void SetFacingDirection()
    {
        if(playerMovement.direction == -1)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(playerMovement.direction == 1)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void ChooseAnimationToPlay()
    {
        string animationToPlay = "";

        //figure out which animiation to play 
        if(playerMovement.rb.linearVelocity.x >= minDetectedInput)
        {
            //moving, play walk animation 
            if(character.currentEqipment == PlayerEquipment.Sword)
            {
                animationToPlay = swordWalkR;
            }
            else if(character.currentEqipment == PlayerEquipment.Shield)
            {
                animationToPlay = shieldWalkR;
            }

        }
        else
        {
            //idle, play idle animation 
            if (character.currentEqipment == PlayerEquipment.Sword)
            {
                animationToPlay = swordIdleR;
            }
            else if (character.currentEqipment == PlayerEquipment.Shield)
            {
                animationToPlay = shieldIdleR;
            }
        }
        

        if (animationToPlay != lastAnimation)
        {
            PlayAnimationByName(animationToPlay);
        }
        
    }

}
