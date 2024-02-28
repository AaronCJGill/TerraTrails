using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //room starts
    //spawn enemies that are supposed to spawn on start
    //have list of enemies to spawn for each room
    //spawn enemy from list
    [SerializeField]
    private List<enemySpawn> StartSpawnList = new List<enemySpawn>();
    [SerializeField]
    private List<enemySpawn> GameSpawnList = new List<enemySpawn>();
    [SerializeField][Tooltip("Should this spawn enemies in the middle of play")]
    bool UseGameSpawnList = true;
    [SerializeField]
    bool SpawnRoutineRestarts = true;
    [SerializeField]
    private List<GameObject> SpawnedEnemies = new List<GameObject>();

    float currentWaitTime = 0;
    float timer = 0;
    bool canSpawn = true;

    int currentPos = 0;

    bool listExhausted = false;

    public static SpawnManager instance;
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        //Debug.Log("Awoken");
    }
    private void Start()
    {
        if (StartSpawnList.Count == 0)
            Debug.Log("Start Spawn List is empty!");
        if (GameSpawnList.Count == 0)
            Debug.Log("Game Spawn List is empty!");

        startRoutine();
        spawnRoutine();
        
    }

    //called at the start of the level
    void startRoutine()
    {
        foreach (enemySpawn e in StartSpawnList)
        {
            createEnemy(e);
        }
        //set the currentWaitTime to whatever is the first in the list
        currentWaitTime = GameSpawnList[0].waitTime ;
    }

    private void Update()
    {
        spawnRoutine();
    }
    //keeps track of when enemies are called
    void spawnRoutine()
    {
        if (canSpawn && UseGameSpawnList)
        {

            if (currentPos >= GameSpawnList.Count && SpawnRoutineRestarts)
            {
                //Debug.Log("At end of spawn list - restarting ");
                currentPos = 0;
            }
            else if (currentPos >= GameSpawnList.Count && !SpawnRoutineRestarts)
            {
                //do nothing but exhaust list
                //Debug.Log("Exhausted list. pos == " + currentPos);
                listExhausted = true;
            }

            if (currentPos < GameSpawnList.Count && !listExhausted)
            {
                //Debug.Log(timer);
                timer += Time.deltaTime;
            }

            //when the timer is greater than the max wait time, set a new max wait time
            if (timer >= currentWaitTime && !listExhausted)
            {
                //spawn the thing we are currently on

                createEnemy(GameSpawnList[currentPos]);

                currentWaitTime = GameSpawnList[currentPos].waitTime;

                currentPos++;
                timer = 0;
            }
        }
    }

    void createEnemy(enemySpawn e)
    {
        GameObject spawnedEnemy;
        if (e.randomPosition)
        {
            //spawning at a random position
            spawnedEnemy = Instantiate(e.enemyPrefab, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0), Quaternion.identity);
        }
        else
        {
            //spawning at a selected position in game
            spawnedEnemy = Instantiate(e.enemyPrefab, new Vector3(e.spawnPoint.x, e.spawnPoint.y, 0), Quaternion.identity);
        }
        SpawnedEnemies.Add(spawnedEnemy);
    }
    public static void levelEnd()
    {
        foreach (GameObject enemy in instance.SpawnedEnemies)
        {
            Destroy(enemy);
        }
        instance.canSpawn = false;
    }

}

[System.Serializable]
public class enemySpawn
{
    [Header("Enemy to be spawned")]
    public GameObject enemyPrefab;
    [Header("How this is spawned")]
    //change this to enum control later
    [Tooltip("if true, this will spawn in at a random position, if not, it will spawn at the coordinates below.")]
    public bool randomPosition;
    [Tooltip("Position this will spawn at. Can be ignored if randomPosition is true.")]
    public Vector2 spawnPoint;

    //amount of time this thing waits until it spawns
    [Header("Time this takes to spawn")]
    public float waitTime = 0f;
}
