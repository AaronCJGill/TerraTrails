using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    GameObject floorOneObjects;
    [SerializeField]
    TextMeshProUGUI timeText, deathsText;

    LevelSelect[] allLevelSelectOptions;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        allLevelSelectOptions = GameObject.FindObjectsByType<LevelSelect>(FindObjectsSortMode.None);
    }

    //checks what map is unlocked and provides access to whichever floor the player desires

    public void clearGameDataButton()
    {
        LevelStatsManager.instance.clearAllLevelStats();
        resetAllLevels();
    }

    void resetAllLevels()
    {
        for (int i = 0; i < allLevelSelectOptions.Length; i++)
        {
            allLevelSelectOptions[i].checkStartingConditions(gameObject);
        }
    }

    public void goToMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void optionsButton()
    {
        //SceneManager.LoadScene("");
    }

    private void Update()
    {


        timeText.text = "time: " + string.Format("{0:0.00}", LevelStatsManager.totalTimeValue);
        deathsText.text = "" + LevelStatsManager.totalDeathCount;

        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < allLevelSelectOptions.Length; i++)
            {
                Debug.Log(allLevelSelectOptions[i].name);
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            resetAllLevels();

        }
    }
}
