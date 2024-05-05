using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField]
    TextMeshProUGUI timeText, deathsText;

    LevelSelect[] allLevelSelectOptions;
    [SerializeField]
    GameObject shopParent, mapParent;

    public AudioClip snClick;

    AudioSource _as;

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
        _as = GetComponent<AudioSource>();
    }

    //checks what map is unlocked and provides access to whichever floor the player desires

    public void clearGameDataButton()
    {
        LevelStatsManager.instance.clearAllLevelStats();

        //levelOne tutorial deletion
        if (PlayerPrefs.HasKey("Industrial_Scene1TutorialDone"))
        {
            Debug.Log("tutorial 1 reset ");
            PlayerPrefs.SetInt("Industrial_Scene1TutorialDone", 0);
        }

        _as.PlayOneShot(snClick, .8f);

        resetAllLevels();
    }

    void resetAllLevels()
    {
        for (int i = 0; i < allLevelSelectOptions.Length; i++)
        {
            allLevelSelectOptions[i].checkStartingConditions(gameObject);
            allLevelSelectOptions[i].lockLevel();
        }
    }

    public void goToMainMenuButton()
    {
        _as.PlayOneShot(snClick, .8f);
        SceneManager.LoadScene(0);
    }
    public void optionsButton()
    {
        //SceneManager.LoadScene("");
    }

    public void spendButton()
    {
        LevelStatsManager.buyLevel(5);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //LevelStatsManager.instance.increaseTotalTimeCheat();
        }

        timeText.text = "time: " + string.Format("{0:0.00}", LevelStatsManager.totalTimeValue);
        deathsText.text = "" + LevelStatsManager.totalDeathCount;

        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < allLevelSelectOptions.Length; i++)
            {
                //Debug.Log(allLevelSelectOptions[i].name);
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //resetAllLevels();

        }
    }

    public void showMap()
    {
        shopParent.SetActive(false);
        mapParent.SetActive(true);

        _as.PlayOneShot(snClick, .8f);
    }
    public void showShop()
    {
        mapParent.SetActive(false);
        shopParent.SetActive(true);
        MapShopManager.instance.shopActivated();

        _as.PlayOneShot(snClick, .8f);
    }

    public void clickFX()
    {
        _as.PlayOneShot(snClick, .8f);
    }
}
