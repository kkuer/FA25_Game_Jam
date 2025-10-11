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


    void Start()
    {
        SpawnBlack();
        SpawnWhite();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnWhite()
    {
        GameObject playerWhite = Instantiate (playerCharacterPrefab, spawnPos[0], Quaternion.identity);
        playerWhite.GetComponent<PlayerCharacter>().colorType = whiteTag;
        playerWhite.GetComponent<CharacterMovement>().inputType = CharacterMovement.ControlScheme.Arrows;

        players.Add(playerWhite.GetComponent<PlayerCharacter>());
    }
    private void SpawnBlack()
    {
        GameObject playerBlack = Instantiate(playerCharacterPrefab, spawnPos[1], Quaternion.identity);
        playerBlack.GetComponent<PlayerCharacter>().colorType = blackTag;
        playerBlack.GetComponent<CharacterMovement>().inputType = CharacterMovement.ControlScheme.WASD;

        players.Add (playerBlack.GetComponent<PlayerCharacter>());
    }
}
