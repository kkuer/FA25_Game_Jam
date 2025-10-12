using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager instance { get; private set; }

    public Slider whiteHealth;
    public Slider blackHealth;

    [SerializeField] private Image whiteHealthFill;
    [SerializeField] private Image blackHealthFill;

    [SerializeField] private TMP_Text whiteDeathCooldown;
    [SerializeField] private TMP_Text blackDeathCooldown;

    private PlayerCharacter white;
    private PlayerCharacter black;

    [SerializeField] private Color deadColor;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (white == null || black == null)
        {
            black = MasterCharacterManager.instance.players[0].gameObject.GetComponent<PlayerCharacter>();
            white = MasterCharacterManager.instance.players[1].gameObject.GetComponent<PlayerCharacter>();
        }

        if (white != null && black != null)
        {
            whiteHealth.value = white.playerCurrentHP;
            blackHealth.value = black.playerCurrentHP;
        }

        if (white.healthState == PlayerCharacter.HealthState.Downed)
        {
            whiteHealthFill.color = deadColor;
            whiteDeathCooldown.gameObject.SetActive(true);
            whiteDeathCooldown.text = Mathf.Ceil(white.respawnTime).ToString();
        }
        else
        {
            whiteHealthFill.color = Color.white;
            whiteDeathCooldown.gameObject.SetActive(false);
        }

        if (black.healthState == PlayerCharacter.HealthState.Downed)
        {
            blackHealthFill.color = deadColor;
            blackDeathCooldown.gameObject.SetActive(true);
            blackDeathCooldown.text = Mathf.Ceil(black.respawnTime).ToString();
        }
        else
        {
            blackHealthFill.color = Color.white;
            blackDeathCooldown.gameObject.SetActive(false);
        }
    }
}
