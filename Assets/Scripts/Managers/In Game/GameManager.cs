using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static bool GameOver;

    public bool timerActive = false;
    private float timer;
    public float Timer
    {
        get { return timer; }
    }
    private float maxTime;
    [SerializeField]
    private GameObject GameUIParent;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI goalTimeText;


    public static GameManager instance;
    private levelType currentLevelType;

    private bool gameOverTriggered = false;

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
        //DontDestroyOnLoad(gameObject);

        if (timerText == null)
        {
            timerText = GameObject.Find("GameCanvas").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Debug.Log("Found timerText");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        checkIfNull();
        
        currentLevelType = LevelInfo.instance.LevelType;

    }
    private void checkIfNull()
    {
        if (GameUIParent == null)
        {
            GameUIParent = GameObject.Find("GameCanvas");
            //Debug.Log("GameCanvas Found");
        }
        if (goalTimeText == null)
        {
            goalTimeText = GameObject.Find("Goal Timer").GetComponent<TextMeshProUGUI>();
            //Debug.Log("GameCanvas Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Health.isDead && gameOverTriggered == false)
        {
            GameOverSequence(true);
            gameOverTriggered = true;
        }
        doTimer();
    }


    public static void showGameCanvas()
    {
        //game has started, allow 
        instance.GameUIParent.SetActive(true);
    }


    public void GameOverSequence(bool playerDied = false)
    {
        if (currentLevelType == levelType.arcade)
        {
            Health.instance.GameOverSequence();
            //if new record is set, then add new time 
            if (LevelInfo.instance.thisLevelsStats.maxTimeCounter < Timer)
            {
                //this is where the bug was likely occuring and adding in more time for no reason
                //LevelStatsManager.instance.increaseTotalTime(LevelInfo.instance.thisLevelsStats.maxTimeCounter, Timer);
            }
            if (playerDied)
            {
                //GameEnded.instance.showScreen(playerDied);
                uiManager.instance.endGame(true);

                //float goalTime, float actualTime, float timeGained
                //if hte player did not beat the dev time show the lesser time
                float goaltime = (LevelInfo.instance.minTime < timer) ? LevelInfo.instance.devTime : LevelInfo.instance.minTime;
                //if the player has done better than the current time, then send in the difference in time
                float timegained = (LevelInfo.instance.thisLevelsStats.maxTimeCounter < timer) ? timer - LevelInfo.instance.thisLevelsStats.maxTimeCounter : 0;

                LevelStatsManager.instance.sendOverGameOverInfo(goaltime, timer, timegained);
                LevelInfo.instance.levelEnd(playerDied);
                PlayerMovement.instance.doKillAnim();
            }
            else
            {
                uiManager.instance.endGame(false);
                //GameEnded.instance.showScreen();
                float goaltime = (LevelInfo.instance.minTime < timer) ? LevelInfo.instance.devTime : LevelInfo.instance.minTime;
                //if the player has done better than the current time, then send in the difference in time
                float timegained = (LevelInfo.instance.thisLevelsStats.maxTimeCounter < timer) ? timer - LevelInfo.instance.thisLevelsStats.maxTimeCounter : 0;

                LevelStatsManager.instance.sendOverGameOverInfo(goaltime, timer, timegained);
                uiManager.instance.endGame(playerDied);
                LevelInfo.instance.levelEnd();
            }
            GameOver = true;
            PlayerMovement.instance.canMove = false;
            timerActive = false;
            SpawnManager.levelEnd();
        }
        else if (currentLevelType == levelType.boss)
        {
            if (playerDied)
            {
                GameEnded.instance.showScreen(playerDied);
                LevelInfo.instance.levelEnd(true);
                PlayerMovement.instance.doKillAnim();
            }
            else
            {
                GameEnded.instance.showScreen();
                LevelInfo.instance.levelEnd();
            }
            GameOver = true;
            PlayerMovement.instance.canMove = false;
            timerActive = false;
            SpawnManager.levelEnd();
        }
        
    }

    public void DeathScreenExitButton()
    {
        SceneManager.LoadScene("MapLevelSelect");
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void mapButton()
    {
        SceneManager.LoadScene(2);
    }



    void doTimer()
    {
        if (timerActive)
        {
            if (currentLevelType == levelType.arcade)
            {
                //counts up, level never stops spawning enemies, maybe only change spawn routine
                timer += Time.deltaTime;
                timerText.text = string.Format("{0:0.00}", timer);
                if (timer >= LevelInfo.instance.devTime)
                {
                    //passing devtime causes counter to become gold
                    timerText.color = Color.yellow;
                }
                else if (timer >= LevelInfo.instance.minTime)
                {
                    timerText.color = Color.grey;
                    goalTimeText.text = LevelInfo.instance.devTime + ".00";
                    DoorScript.levelOver();
                }
                else if (timer <= LevelInfo.instance.minTime)
                {
                    //not passed any time - still attempting level
                    //timerText.color = Color.black;
                    goalTimeText.text = LevelInfo.instance.minTime + ".00";
                }

            }
            else if (currentLevelType == levelType.boss)
            {
                //counts down, use old code
                if (timer <= 0)
                {
                    DoorScript.levelOver();
                    SpawnManager.levelEnd();
                    timerText.text = "Go to the exit!";
                }
                else
                {
                    timer -= Time.deltaTime;
                    timerText.text = string.Format("{0:0.00}", timer);
                }
            }

        }
    }
    public void updateHealthUI()
    {
        if (Health.OneHitKill)
        {
            HealthUI.disableHealthUI();
        }
        else
        {
            HealthUI.instance.updateHealthUI();
        }
    }

    public static void levelStart(float t)
    {
        
            //count down from max
            instance.timerActive = true;
            instance.maxTime = t;
            instance.timer = t;
        
    }
    public static void levelStart()
    {
        if (instance.currentLevelType == levelType.arcade)
        {
            //count up from 0
            instance.timer = 0;
            instance.timerActive = false;
        }
        else
        {
            instance.timerActive = false;
            instance.maxTime = 1000000;
        }

    }


}

namespace usefulFunctions
{
    public static class positioning{

        public static Vector3 PickRandomPointNearby(Vector3 pos, float radius)
        {
            var point = Random.insideUnitSphere * radius;
            point.y = 0;
            point += pos;
            return point;
        }

        public static Vector3 getRandomSpawnPoint()//if bounds are not set, then gives a random position between 0 and 10
        {
            Vector3 spawnPoint;
            if (LevelBoundary.leftTopBound != null && LevelBoundary.BottomRightBound != null)
            {
                float randomXPos = Random.Range(LevelBoundary.leftTopBound.X, LevelBoundary.BottomRightBound.X);
                float randomYPos = Random.Range(LevelBoundary.leftTopBound.Y, LevelBoundary.BottomRightBound.Y);

                spawnPoint = new Vector3(randomXPos, randomYPos, 0);

            }
            else
            {
                spawnPoint = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            }

            return spawnPoint;
        }


        public static Vector3 getFreePosition()
        {
            Vector3 spawnPoint;
            for (int i = 0; i < 100; i++)
            {
                float randomXPos = Random.Range(LevelBoundary.leftTopBound.X, LevelBoundary.BottomRightBound.X);
                float randomYPos = Random.Range(LevelBoundary.leftTopBound.Y, LevelBoundary.BottomRightBound.Y);

                spawnPoint = new Vector3(randomXPos, randomYPos, 0);
                Collider[] hits = Physics.OverlapSphere(spawnPoint, 1, 0, QueryTriggerInteraction.Ignore);
                Debug.Log("Finding point");
                if (hits.Length == 0)
                {
                    Debug.Log("Found point");
                    //Gizmos.DrawCube(spawnPoint, Vector3.one);
                    return spawnPoint;
                }
            }

            return Vector3.one;
        }
        public static Vector3 getFreePosition(Vector3 pos)
        {
            Vector3 spawnPoint;
            for (int i = 0; i < 100; i++)
            {
                float randomXPos = Random.Range(LevelBoundary.leftTopBound.X, LevelBoundary.BottomRightBound.X);
                float randomYPos = Random.Range(LevelBoundary.leftTopBound.Y, LevelBoundary.BottomRightBound.Y);

                spawnPoint = new Vector3(randomXPos, randomYPos, 0);
                Collider[] hits = Physics.OverlapSphere(spawnPoint, 1, 6, QueryTriggerInteraction.Ignore);
                Debug.Log("Finding point");
                if (hits.Length == 0)
                {
                    Debug.Log("Found point");
                    //Gizmos.DrawCube(spawnPoint, Vector3.one);
                    return spawnPoint;
                }
            }
            float randomXPos2 = Random.Range(LevelBoundary.leftTopBound.X, LevelBoundary.BottomRightBound.X);
            float randomYPos2 = Random.Range(LevelBoundary.leftTopBound.Y, LevelBoundary.BottomRightBound.Y);
            Debug.Log("NonFreePoint found");
            return new Vector3(randomXPos2, randomYPos2, 0); 
        }
    }

}

