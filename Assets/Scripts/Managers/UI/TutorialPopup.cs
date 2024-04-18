using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPopup : MonoBehaviour
{

    //reference to the tutorial to pop up
    //pauses the game, so no scripts play
    //waits for player to press jump/space/X
    //unpauses game and deletes itself
    // Start is called before the first frame update
    [SerializeField]
    GameObject tutorialObject;
    bool tutorialActive = false;
    //string is set to
    string tutorialSaveString;
    void Start()
    {
        tutorialSaveString = SceneManager.GetActiveScene().name + "TutorialDone";

        tutorialObject.SetActive(false);
        pausegameBehavior();
    }

    void pausegameBehavior()
    {
        //saves the level name and if the player has played this level before to level prefs
        //uses the name of the level and a bool to check if tutorial needs to play
        if (PlayerPrefs.HasKey(tutorialSaveString))
        {
            if (PlayerPrefs.GetInt(tutorialSaveString) == 0)
            {
                //tutorial has not been activated
                tutorialObject.SetActive(true);
                PlayerPrefs.SetInt(tutorialSaveString, 1);
                tutorialActive = true;
                Time.timeScale = 0;

            }
            else
            {
                //tutorial has been activated
                //dont do anything
                tutorialActive = false;
            }
        }
        else
        {
            //the tutorial has never been activated, activate it now
            tutorialObject.SetActive(true);
            PlayerPrefs.SetInt(tutorialSaveString, 1);
            tutorialActive = true;
            Time.timeScale = 0;

        }

    }
    void resetTutorial()
    {
        if (PlayerPrefs.HasKey(tutorialSaveString))
        {
            PlayerPrefs.SetInt(tutorialSaveString, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialActive && Input.GetAxisRaw("Jump") == 1)
        {
            //remove tutorial
            tutorialObject.SetActive(false);
            tutorialActive = false;
            Time.timeScale = 1;

        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            resetTutorial();//resets the tutorial on this level
        }
    }
}
