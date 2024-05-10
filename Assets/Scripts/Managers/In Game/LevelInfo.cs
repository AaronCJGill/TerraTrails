using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum levelType
{
    arcade,
    boss
}
public class LevelInfo : MonoBehaviour
{
    [Header("Information about the level")]
    [Tooltip("If this is true, then the level has a time limit. Set to false on menu.")]
    [SerializeField]
    private levelType levelType;
    public bool isTimed = true;
    [Header("Boss Level Settings")]
    [Tooltip("amount of time this will take if it is a boss room")]
    public float bossTime;
    [Header("Arcade level settings")]
    [Tooltip("minimum amount of time for player to complete level")]
    public float minTime;
    [Tooltip("Time the player should aim for when completing level")]
    public float devTime;

    public levelStats thisLevelsStats;
    public string levelName = "";

    public int deathCounter;

    public levelType LevelType
    {
        get { return levelType; }
    }
    public static LevelInfo instance;
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

        //if levelname is unsaved, then save levelname
        if (levelName == "")
            levelName = SceneManager.GetActiveScene().name;
        getLevelStats();
    }

    private void Start()
    {
        if (isTimed || LevelType == levelType.boss)
        {
            GameManager.levelStart(bossTime);
        }
        else
        {
            GameManager.levelStart();
        }
    }


    public void getLevelStats()
    {
        //when the level starts, on awake, get the level's stats 
        Debug.Log("This has been set active");
        if (LevelStatsManager.checkIfSaveFileExists(levelName))
        {
            Debug.Log("level loaded");
            thisLevelsStats = LevelStatsManager.instance.load(levelName);
            thisLevelsStats.devTime = devTime;
            thisLevelsStats.minTime = minTime;
        }
        else
        {
            thisLevelsStats = LevelStatsManager.Load(levelName);
            //thisLevelsStats = new levelStats(levelName, 0, 0);
            thisLevelsStats.devTime = devTime;
            thisLevelsStats.minTime = minTime;
            Debug.Log("New level created");
        }

        if (thisLevelsStats.maxTimeCounter == 0 || thisLevelsStats.minTime == 0)
        {
            //level has not been played before
            thisLevelsStats = new levelStats(levelName, 0, 0);
            thisLevelsStats.devTime = devTime;
            thisLevelsStats.minTime = minTime;
        }
        //this either returns a new level or a saves the current level info
    }



    public void levelEnd(bool died = false)
    {
        //save level stats
        /*
        if (thisLevelsStats.maxTimeCounter < GameManager.instance.Timer)
        {
            thisLevelsStats.maxTimeCounter = GameManager.instance.Timer;
        }*/
        //I think updating time is handled in the save function
        thisLevelsStats.devTime = devTime;
        thisLevelsStats.minTime = minTime;
        thisLevelsStats.maxTimeCounter = GameManager.instance.Timer;
        LevelStatsManager.instance.save(thisLevelsStats, died);

    }


}

