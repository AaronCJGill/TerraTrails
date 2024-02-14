using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    public static DeathCounter instance;
    static int globalDeaths;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //create variables if they dont exist
        //variables for each level
        //Global
        //LevelOneDeaths, LevelTwoDeaths, etc

    }


    public void clearDeaths()
    {

    }
}
