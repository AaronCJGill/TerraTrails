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
    
    void gameOverUI()
    {

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
        //Debug.Log(Health.instance.health);
        /*
        for (int i = 0; i < healthUI.Length; i++)
        {
            //if health is == healthui
            if (Health.instance.health > i)
            {
                healthUI[i].SetActive(true);
            }
            else
            {
                healthUI[i].SetActive(false);
            }
        }
        */
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
