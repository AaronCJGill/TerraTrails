using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}
