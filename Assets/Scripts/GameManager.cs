using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum enemyType
{
    Knight,
    Bishop,
    Rook
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    public bool gameActive;
    public bool gamePaused;

    public GameObject activeRoom;

    public List<GameObject> roomPrefabs = new List<GameObject>();
    public List<GameObject> enemyPrefabsW = new List<GameObject>();
    public List<GameObject> enemyPrefabsB = new List<GameObject>();

    public List<GameObject> activeEnemies = new List<GameObject>();

    public int difficultyScaling;
    public float gameSpeed;
    public bool levelCleared;

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

    private void Start()
    {
        activeRoom = null;
        levelCleared = false;
        gameActive = true;

        difficultyScaling = 1;
        gameSpeed = 1f;

        initializeNewRoom(difficultyScaling);
        FadeTransition.instance.fadeOut();
    }

    private void Update()
    {
        if (gameActive && !gamePaused)
        {
            Time.timeScale = gameSpeed;
        }

        if (levelCleared == true && activeRoom != null)
        {
            levelCleared = false;
            StartCoroutine(levelTransition());
        }
    }

    public void initializeNewRoom(int enemyAmount)
    {
        GameObject chosenRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        GameObject newRoom = Instantiate(chosenRoom, Vector3.zero, Quaternion.identity, gameObject.transform);

        activeRoom = newRoom;

        Room r = newRoom.GetComponent<Room>();

        //move players to spawns
        if (MasterCharacterManager.instance.players.Count == 2)
        {
            MasterCharacterManager.instance.players[0].gameObject.transform.position = r.blackSpawn.transform.position;
            MasterCharacterManager.instance.players[1].gameObject.transform.position = r.whiteSpawn.transform.position;
        }

        //enemy spawner
        if (r != null)
        {
            for (int i = 0; i < enemyAmount; i++)
            {
                if (i % 2 == 0)
                {
                    //spawn white piece
                    GameObject newEnemy = Instantiate(enemyPrefabsW[Random.Range(0, enemyPrefabsW.Count)],
                        r.enemySpawns[Random.Range(0, r.enemySpawns.Count)].position,
                        Quaternion.identity);
                    activeEnemies.Add(newEnemy);
                }
                else
                {
                    //spawn black piece
                    GameObject newEnemy = Instantiate(enemyPrefabsB[Random.Range(0, enemyPrefabsB.Count)],
                        r.enemySpawns[Random.Range(0, r.enemySpawns.Count)].position,
                        Quaternion.identity);
                    activeEnemies.Add(newEnemy);
                }
            }
        }
    }

    private IEnumerator levelTransition()
    {
        FadeTransition.instance.fadeIn(null);
        yield return new WaitForSeconds(1);

        if (activeEnemies.Count > 0)
        {
            foreach (GameObject e in activeEnemies.ToList())
            {
                if (e != null)
                {
                    Destroy(e);
                }
            }
            activeEnemies.Clear();
        }

        Destroy(activeRoom.gameObject);
        activeRoom = null;

        if (difficultyScaling <= 10)
        {
            difficultyScaling += 1;
        }

        Time.timeScale = gameSpeed;

        initializeNewRoom(difficultyScaling);
        yield return new WaitForSeconds(0.5f);
        FadeTransition.instance.fadeOut();
    }
}
