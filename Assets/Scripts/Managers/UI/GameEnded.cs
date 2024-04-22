using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameEnded : MonoBehaviour
{
    public static GameEnded instance;

    [SerializeField]
    private GameObject GameOverUI;
    [SerializeField]
    private TextMeshProUGUI topGameOverText;
    [SerializeField]
    private TextMeshProUGUI timeStatsText;
    [SerializeField]
    private TextMeshProUGUI GameOverText;
    bool textGenerated = false;

    [SerializeField]
    private GameObject nextLevelButtonGameObect;

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
        GameOverUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            restartButton();
        }
    }

    public void gotomainmenu()
    {
        //SceneManager.
        //0);
        SceneManager.LoadScene("MapLevelSelect");
    }
    public void goToMapButton()
    {
//        SceneManager.LoadScene(0);
        SceneManager.LoadScene("MapLevelSelect");

    }
    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void nextLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    float waitTimeTotal = 1.5f;
    float waitTimeCurrent = 0;
    IEnumerator showScreenRoutine(bool playerDied = false)
    {
        if (playerDied)
            yield return new WaitForSeconds(waitTimeTotal);
        
        GameOverUI.SetActive(true);
        if (LevelInfo.instance.LevelType == levelType.arcade)
        {
            if (GameManager.instance.Timer >= LevelInfo.instance.minTime || LevelInfo.instance.thisLevelsStats.maxTimeCounter >= LevelInfo.instance.minTime)
            {
                //if we die, display phyric victory
                //we beat min time and its an arcade level
                topGameOverText.text = "Level passed";
                if (playerDied)
                    GameOverText.text = "pyrrhic victory";
                else
                    GameOverText.text = "Head on soldier";
                //nextLevelButtonGameObect.SetActive(true);

                //if the level has been passed, show the option for a dev time
                timeStatsText.text = "Time Needed: " + LevelInfo.instance.minTime
                    + "\nTime Achieved: " + GameManager.instance.Timer
                    + "\nGold Time: " + LevelInfo.instance.devTime;
            }
            else
            {
                //player died and time is not up

                topGameOverText.text = "You Died";

                //nextLevelButtonGameObect.SetActive(false);
                if (GameOverTextGenerator.instance != null)
                    GameOverText.text = GameOverTextGenerator.instance.generateString();
                else
                    GameOverText.text = "Get Better!";
                timeStatsText.text = "Time Needed: " + LevelInfo.instance.minTime
    + "\nTime Achieved: " + GameManager.instance.Timer;
            }
        }
        else if (LevelInfo.instance.LevelType == levelType.boss)
        {
            //boss level, have to either be at door or die
            if (GameManager.instance.Timer >= LevelInfo.instance.bossTime)
            {
                //player hit door
                topGameOverText.text = "Boss Survived";
                GameOverText.text = "Head on soldier";
                //nextLevelButtonGameObect.SetActive(true);
            }
            else
            {
                //player died 
                topGameOverText.text = "You Died";
                GameOverText.text = GameOverTextGenerator.instance.generateString();
            }
        }

    }
    public void showScreen(bool playerDied = false)
    {
        StartCoroutine(showScreenRoutine(playerDied));
    }
}
