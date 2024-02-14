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
    private float maxTime;

    [SerializeField]
    private GameObject GameUIParent;
    [SerializeField]
    private TextMeshProUGUI timerText;
    public static GameManager instance;

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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        checkIfNull();
    }
    private void checkIfNull()
    {
        if (GameUIParent == null)
        {
            GameUIParent = GameObject.Find("GameCanvas");
            Debug.Log("GameCanvas Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Health.isDead)
        {
            GameOverSequence(true);
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
        if (playerDied)
            GameEnded.instance.showScreen(playerDied);
        GameOver = true;
        PlayerMovement.instance.canMove = false;
        timerActive = false;
        SpawnManager.levelEnd();
    }

    public void DeathScreenExitButton()
    {
        SceneManager.LoadScene(0);
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
        instance.timerActive = true;
        instance.maxTime = t;
        instance.timer = t;
    }
    public static void levelStart()
    {
        instance.timerActive = false;
        instance.maxTime = 1000000;
    }


}
