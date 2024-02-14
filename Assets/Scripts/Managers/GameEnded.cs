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
    private TextMeshProUGUI GameOverText;
    bool textGenerated = false;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Escape))
        {
            gotomainmenu();
        }
    }

    public void gotomainmenu()
    {
        SceneManager.LoadScene(0);
    }
    public void goToMapButton()
    {
        SceneManager.LoadScene(0);
    }
    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void nextLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void showScreen(bool playerDied = false)
    {
        //TODO: check if level completed first
        GameOverUI.SetActive(true);
        if (playerDied)
        {
            GameOverText.text = GameOverTextGenerator.instance.generateString();
        }
        else
        {
            GameOverText.text = "You passed";
        }
    }
}
