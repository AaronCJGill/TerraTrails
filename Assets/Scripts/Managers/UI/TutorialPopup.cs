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
    public static TutorialPopup instance;
    [SerializeField]
    bool shopTutorial = false;
    public static bool TutorialActive
    {
        get
        {
            if (instance == null)
            {
                return false;
            }
            else
            {
                return instance.tutorialActive;
            }
        }
    }
    //string is set to
    string tutorialSaveString;
    void Start()
    {
        if (shopTutorial)
        {
            pausegameBehavior();
        }
        else
        {
            tutorialSaveString = SceneManager.GetActiveScene().name + "TutorialDone";
            Debug.Log(tutorialSaveString);
            tutorialObject.SetActive(false);
            pausegameBehavior();
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        
    }

    void pausegameBehavior()
    {
        //saves the level name and if the player has played this level before to level prefs
        //uses the name of the level and a bool to check if tutorial needs to play
        if (!shopTutorial)
        {
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
        else
        {
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
        if (shopTutorial)
        {
            if (tutorialActive && Input.GetAxisRaw("Jump") == 1)
            {
                //remove tutorial
                tutorialActive = false;
                Time.timeScale = 1;
                gameObject.SetActive(false);
            }
        }
        else
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
                //resetTutorial();//resets the tutorial on this level
            }
        }
        
    }
}
