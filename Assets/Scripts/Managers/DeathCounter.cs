using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    public static DeathCounter instance;
    public static int globalDeaths;


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


    private void checkForSavedDeaths()
    {
        //first floor has seven levels, including boss and 
        


        //get level name from the levelinfo script in each level

        //options
        //if statement for each - playerdata.getint("F1L1")
        //
   
    
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
