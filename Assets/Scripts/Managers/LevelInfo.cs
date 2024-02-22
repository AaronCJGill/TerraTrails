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
        thisLevelsStats = LevelStatsManager.instance.load(levelName);
        //this either returns a new level or a saves the current level info
    }

    public void levelEnd(bool died = false)
    {
        //save level stats
        if (thisLevelsStats.maxTimeCounter < GameManager.instance.Timer)
        {
            thisLevelsStats.maxTimeCounter = GameManager.instance.Timer;
        }
        if (died)
        {
            LevelStatsManager.instance.increaseTotalDeaths();
            thisLevelsStats.totalDeathCounter++;
        }

        LevelStatsManager.instance.save(thisLevelsStats);
    }


}

