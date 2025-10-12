using UnityEngine;

public class LevelEndDoor : MonoBehaviour
{
    [Header("Standard Settings")]
    public bool ReadyToSwitch = false;
    public float PlayersInDoor = 0f;
    private bool enemiesDefeated = false;

    [Header("Debug Settings")]
    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private Color interactableColor = Color.green;
    [SerializeField] private Color defaultColor = Color.white;

    private bool debounce;
    public GameObject vortex;

    private void Start()
    {
        debounce = false;
    }

    private void Update()
    {
        if (GameManager.instance != null)
        {
            // Check if all enemies have been defeated in GameManager
            enemiesDefeated = GameManager.instance.activeEnemies.Count == 0;

            CheckForCompletion();
        }

        UpdateDebugSpriteColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if (player != null)
        {
            PlayersInDoor += 1f;
            //Debug.Log("Player entered door. Players in door: " + PlayersInDoor);
            CheckForCompletion();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
        if (player != null)
        {
            PlayersInDoor = Mathf.Max(0, PlayersInDoor - 1f);
            //Debug.Log("Player exited door. Players in door: " + PlayersInDoor);
            CheckForCompletion();
        }
    }

    private void CheckForCompletion()
    {
        // ReadyToSwitch can only be true if enemies are defeated
        if (!enemiesDefeated)
        {
            ReadyToSwitch = false;
            return;
        }

        int totalPlayers = MasterCharacterManager.instance != null
            ? MasterCharacterManager.instance.players.Count
            : 2;

        ReadyToSwitch = PlayersInDoor >= totalPlayers;
        vortex.SetActive(true);

        if (ReadyToSwitch && GameManager.instance != null && debounce == false)
        {
            debounce = true;
            GameManager.instance.levelCleared = true;
            //Debug.Log("All players in door and enemies defeated. Level cleared!");
        }
    }


    // Debug function - Feel Free to Remove Later <-------
    private void UpdateDebugSpriteColor()
    {
        if (doorSprite == null) return;

        doorSprite.color = ReadyToSwitch ? interactableColor : defaultColor;
    }
}
