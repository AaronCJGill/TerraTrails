using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private GameObject GameOverUI;
    [SerializeField]
    private TextMeshProUGUI GameOverText;
    //[SerializeField]
    //GameObject[] healthUI = new GameObject[5];
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
        if (GameOverUI == null)
        {
            GameOverUI= GameUIParent.transform.GetChild(2).gameObject;
            Debug.Log("GameOverUI Found");
        }
        if (GameOverText==null)
        {
            GameOverText = GameOverUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            Debug.Log("GameOverText Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Health.isDead)
        {
            GameOverSequence();
        }

        doTimer();

    }


    public static void showGameCanvas()
    {
        //game has started, allow 
        instance.GameUIParent.SetActive(true);
    }


    void GameOverSequence()
    {
        GameOver = true;
        PlayerMovement.instance.canMove = false;
        timerActive = false;
        SpawnManager.levelEnd();
        gameOverUI();
    }
    bool textGenerated = false;

    void gameOverUI()
    {
        //call game over text generator to change game over text
        GameOverUI.SetActive(true);
        if (textGenerated == false)
        {
            GameOverText.text = GameOverTextGenerator.instance.generateString();
            textGenerated = true;
        }
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
        HealthUI.instance.updateHealthUI();

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
