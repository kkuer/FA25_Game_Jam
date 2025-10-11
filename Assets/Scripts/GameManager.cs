using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;

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
        gameActive = true;

        //spawn room
        //spawn enemies
        initializeNewRoom(4);

        //spawn in players
    }

    public void initializeNewRoom(int enemyAmount)
    {
        if (activeRoom != null)
        {
            Destroy(activeRoom.gameObject);
            activeRoom = null;
        }

        GameObject chosenRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        GameObject newRoom = Instantiate(chosenRoom, Vector3.zero, Quaternion.identity, gameObject.transform);

        activeRoom = newRoom;

        Room r = newRoom.GetComponent<Room>();

        //move players to spawns
        MasterCharacterManager.instance.players[0].gameObject.transform.position = r.blackSpawn.transform.position;
        MasterCharacterManager.instance.players[1].gameObject.transform.position = r.whiteSpawn.transform.position;

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
}
