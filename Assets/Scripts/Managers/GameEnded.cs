using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnded : MonoBehaviour
{
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
}
