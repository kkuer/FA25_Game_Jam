using UnityEngine;
using System.Collections.Generic;


public class MasterCharacterManager : MonoBehaviour
{
    public static MasterCharacterManager instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null )
            instance = this;
        else
            Destroy(this);
    }

    public GameObject playerCharacterPrefab;
    public List<PlayerCharacter> players = new List<PlayerCharacter> ();

    public ColorType whiteTag;
    public ColorType blackTag;

    public List<Vector2> spawnPos = new List<Vector2> ();

    public GameObject SwordPrefab;
    public GameObject ShieldPrefab;


    void Start()
    {
        SpawnBlack();
        SpawnWhite();
    }

    Vector2 LastPosSwordPlayer;
    Vector2 LastPosShieldPlayer;

    // Update is called once per frame
    void Update()
    {
        if (players[0].ReadyToSwap && players[1].ReadyToSwap) 
        {
            foreach (PlayerCharacter player in players)
            {
                player.SwapWeapons();
                if (player.previousEquipment == PlayerEquipment.Sword)
                {
                    //LastPosSwordPlayer
                    player.currentEqipment = PlayerEquipment.Shield;

                    //call this where you swap weapons
                    SwapFlash.instance.flash();
                }
                else if (player.previousEquipment == PlayerEquipment.Shield)
                {
                    player.currentEqipment = PlayerEquipment.Sword;

                    //call this where you swap weapons
                    SwapFlash.instance.flash();
                }

                player.ReadyToSwap = false;
            }
        }

        if (players[0].healthState == HealthState.Downed || players[1].healthState == HealthState.Downed)
        {
            if (players[0].healthState == HealthState.Downed && players[1].healthState == HealthState.Downed)
            {
                // game over
            }
        }
    }

    private void SpawnWhite()
    {
        GameObject playerWhite = Instantiate (playerCharacterPrefab, spawnPos[0], Quaternion.identity);
        playerWhite.GetComponent<PlayerCharacter>().colorType = whiteTag;
        playerWhite.GetComponent <PlayerCharacter>().currentEqipment = PlayerEquipment.Sword;
        playerWhite.GetComponent<CharacterMovement>().inputType = ControlScheme.Arrows;

        players.Add(playerWhite.GetComponent<PlayerCharacter>());
    }
    private void SpawnBlack()
    {
        GameObject playerBlack = Instantiate(playerCharacterPrefab, spawnPos[1], Quaternion.identity);
        playerBlack.GetComponent<PlayerCharacter>().colorType = blackTag;
        playerBlack.GetComponent<PlayerCharacter>().currentEqipment = PlayerEquipment.Shield;
        playerBlack.GetComponent<CharacterMovement>().inputType = ControlScheme.WASD;

        players.Add (playerBlack.GetComponent<PlayerCharacter>());
    }

    private void SpawnSword(Vector2 previousPlayer, Vector2 currentPlayer)
    {

    }
    private void SpawnShield()
    {

    }
}
