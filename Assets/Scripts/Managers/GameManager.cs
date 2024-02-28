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
            Debug.Log("GameCanvas Found");
        }
        if (goalTimeText == null)
        {
            goalTimeText = GameObject.Find("Goal Timer").GetComponent<TextMeshProUGUI>();
            Debug.Log("GameCanvas Found");
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
                LevelStatsManager.instance.increaseTotalTime(LevelInfo.instance.thisLevelsStats.maxTimeCounter, Timer);
            }
            if (playerDied)
            {
                GameEnded.instance.showScreen(playerDied);
                LevelInfo.instance.levelEnd(true);
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
        else if (currentLevelType == levelType.boss)
        {
            if (playerDied)
            {
                GameEnded.instance.showScreen(playerDied);
                LevelInfo.instance.levelEnd(true);
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
        SceneManager.LoadScene(0);
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
                    goalTimeText.text = "Gold Time: " + LevelInfo.instance.devTime.ToString();
                    DoorScript.levelOver();
                }
                else if (timer <= LevelInfo.instance.minTime)
                {
                    //not passed any time - still attempting level
                    timerText.color = Color.black;
                    goalTimeText.text = "Min Goal Time: " + LevelInfo.instance.minTime;
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
