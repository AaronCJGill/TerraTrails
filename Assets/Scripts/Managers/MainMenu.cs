using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuParentObject, creditsParentObject;

    public void startGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void creditsButton()
    {
        mainMenuParentObject.SetActive(false);
        creditsParentObject.SetActive(true);
    }
    public void goToMainMenu()
    {
        //holdover - delete when obselete
        SceneManager.LoadScene(0);
    }
    public void quitGame()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitGame();
        }
    }

    public void exitButtons()
    {
        mainMenuParentObject.SetActive(true);
        creditsParentObject.SetActive(false);

    }
}
