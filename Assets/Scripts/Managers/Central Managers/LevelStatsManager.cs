using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelStatsManager : MonoBehaviour
{
    public static string directory = "/SaveData/";
    public static LevelStatsManager instance;

    private string totalDeathString = "totalDeaths";
    public static int totalDeathCount;
    private string totalTimeString = "totalTime";
    public static float totalTimeValue;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        string dir = Application.persistentDataPath + directory;

        //if the directory does not exist
        if (!Directory.Exists(dir))
        {
            Debug.Log("Directory Created");
            Directory.CreateDirectory(dir);
        }

        if (!PlayerPrefs.HasKey("totalTime"))
        {
            PlayerPrefs.SetFloat("totalTime", 0);
            Debug.Log("TotalTime has been made");
        }
        if (!PlayerPrefs.HasKey("totalDeaths"))
        {
            PlayerPrefs.SetInt("totalDeaths", 0);
            Debug.Log("TotalDeaths has been made");
        }


        //load info on awake

        loadGlobalValues();
    }
    


    public void increaseTotalDeaths()
    {

    }
    public void increaseTotalTime(float oldTime, float newTime)
    {
        totalTimeValue = totalTimeValue + (newTime - oldTime);
    }
    public static void Save(levelStats ls, bool playerdied = false)
    {
        instance.save(ls, playerdied);
    }

    
    //each level has its own json file with its stats
    public void save(levelStats ls, bool playerDied = false)
    {
        string dir = Application.persistentDataPath + directory;
        string json;//= JsonUtility.ToJson(ls);

        //handle player death
        if (playerDied)
        {
            ls.totalDeathCounter++;
            totalDeathCount++;
        }

        string filePath = dir + ls.levelName + ".txt";

        //three possibilities
        // file does not exist, then save file without modification
        // file exists and new time was set, save new time and new death stat
        // file exists, and new time was not set, save old time and new death stat


        Debug.Log("Checking if " + filePath + " exists ");

        //if file already exists - then dont overwrite some of the previous information
        //if (checkIfDirectoryExists(filePath))//may change this to checkifsavefileexists
        if(checkIfSaveFileExists(ls.levelName))
        {
            Debug.Log("Filepath exists");
            //we get the previous loaded level
            levelStats ol = load(ls.levelName);
            //check if new time is bigger than old time, if should just add it
            if (newTimeSet(ol, ls))
            {
                //calculate how much time to add
                float timeToAdd = ls.maxTimeCounter - ol.maxTimeCounter;
                Debug.Log("Adding time difference " + timeToAdd + " - - " + totalTimeValue);

                totalTimeValue += timeToAdd;
                Debug.Log("Adding time difference " + timeToAdd + " - - " + totalTimeValue);

                //use new time - all stats are the same so use new level counters
                levelStats savedLevel = new levelStats(ls);

                //save level
                json = JsonUtility.ToJson(savedLevel);
                File.WriteAllText(filePath, json);

                //also communicate with gamemanager to show new time and old time
                Debug.Log("New Time saved");


            }
            else
            {
                //no new time, use old time
                levelStats savedLevel = new levelStats(ls.levelName, ls.totalDeathCounter, ol.maxTimeCounter);

                //save level
                json = JsonUtility.ToJson(savedLevel);
                File.WriteAllText(filePath, json);

                //communicate with manager
                Debug.Log("Old Time not passed Saving over the old levels stats");

            }



            //we can communicate with UI and display new high score thing



        }
        else 
        {
            //Save does not exist
            json = JsonUtility.ToJson(ls);

            if (playerDied)
            {
                ls.totalDeathCounter++;
            }

            //add all time since this is new time
            totalTimeValue += ls.maxTimeCounter;
            File.WriteAllText(filePath, json);

            //communicate with UI manager
            Debug.Log("Save does not exist, creating new save with same name");


        }

        //get json and save it if it does not


        //save playerprefs
        saveGlobalValues();

        Debug.Log("Level Stats saved");
    }

    bool newTimeSet(levelStats oldStats, levelStats newStats)
    {
        //checks if new high score is checked, if it is, add time that was gained since last run

        if (oldStats.maxTimeCounter < newStats.maxTimeCounter)
        {
            Debug.Log("new time added");
            return true;
        }
        else
        {
            Debug.Log("No new time set");
            return false;
        }
    }

    public bool checkIfDirectoryExists(string fullPath)
    {
        //DEPRECATED - Searches for FULLPATH instead of the file thats there USE checkIfSaveFileExists INSTEAD
        //if directory exists, load it. If it doesnt, then dont
        if (File.Exists(fullPath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool checkIfSaveFileExists(string levelName)
    {
        string dir = Application.persistentDataPath + directory;
        string filePath = dir + levelName + ".txt";

        return File.Exists(filePath);
    }
    public bool buyUsingTime(float timeCost)
    {
        //handles all of time buying

        if (timeCost < totalTimeValue)
        {
            totalTimeValue -= timeCost;
            saveGlobalValues();
            return true;
        }
        else
        {
            return false;
        }


    }

    void saveGlobalValues()
    {
        //just saev whatever values have already been changed
        PlayerPrefs.SetFloat(totalTimeString, totalTimeValue);
        PlayerPrefs.SetInt(totalDeathString, totalDeathCount);
        Debug.Log("time value set to : " + totalTimeValue);
    }
    void resetGlobalValues()
    {
        PlayerPrefs.SetFloat(totalTimeString, 0);
        PlayerPrefs.SetInt(totalDeathString, 0);
        totalTimeValue = 0;
        totalDeathCount = 0;
    }
    void loadGlobalValues()
    {
        totalTimeValue = PlayerPrefs.GetFloat(totalTimeString);
        totalDeathCount = PlayerPrefs.GetInt(totalDeathString);
    }

    //returns the level based on levelname
    public static levelStats Load(string levelName)
    {
        if (instance == null)
        {
            Debug.Log("Returning new level");
            return new levelStats(levelName);
        }
        else
        {
            Debug.Log("Instance found");
            return instance.load(levelName);
        }
    }

    public levelStats load(string levelName)
    {
        string dir = Application.persistentDataPath + directory;
        string filePath = dir + levelName + ".txt";
        Debug.Log("Attempting to load file at " + filePath);
        if (checkIfSaveFileExists(levelName))
        {
            string json = File.ReadAllText(filePath);
            levelStats ls = JsonUtility.FromJson<levelStats>(json);
            Debug.Log("Successfully loaded map - " + ls.levelName + " " + ls.maxTimeCounter);
            return ls;
        }
        else
        {
            Debug.Log("Could not load level - New levelStats Made named " + levelName);
            
            return new levelStats(levelName);
        }
    }


    //maybe clear individual level but not sure? how would this work with time currency?
    public void clearAllLevelStats()
    {
        //for each level, delete all data
        string dir = Application.persistentDataPath + directory;
        string[] allFileList = Directory.GetFiles(dir);

        foreach (string item in allFileList)
        {
            if (File.Exists(Application.persistentDataPath + directory + item))
            {
                Debug.Log(item);
            }
            //"C:\Users\jamal\AppData\LocalLow\TerraTrialsMSS\TerraTrials\SaveData\SampleSceneTwo.txt"
            File.Delete(item);
        }
        //reset global stats
        resetGlobalValues();
    }

    public bool deleteLevelStats(levelStats ls)
    {
        //destroys the levels stats
        //subtracts amounts from the global file

        if (totalTimeValue < ls.maxTimeCounter)
        {
            //make level time counter = 0
            totalTimeValue -= ls.maxTimeCounter;
            totalDeathCount -= ls.totalDeathCounter;

            ls = new levelStats(ls.levelName, 0, 0);//reset values
            save(ls);//save the values we have changed
            saveGlobalValues();
            return true;
        }
        else
        {
            //dont allow stats to be deleted
            return false;
        }
    }

    public levelStats[] returnAllLevels()
    {
        levelStats[] lss;

        //find all the levels in the directory and return them

        return new levelStats[1];
    }


    public static bool canBuyLevel(float cost)
    {
        return PlayerPrefs.GetFloat(instance.totalTimeString) > cost;
    }
    public static void buyLevel(float cost)
    {
        if (canBuyLevel(cost))
        {
            totalTimeValue -= cost;
            PlayerPrefs.SetFloat(instance.totalTimeString, totalTimeValue - cost);
            Debug.Log("time value set to : " + totalTimeValue);
            instance.saveGlobalValues();
        }
    }



    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Z))
        {
            clearAllLevelStats();
            Debug.Log("levels cleared");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            increaseTotalTime(0, 50);
            saveGlobalValues();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(load("SampleSceneTwo").levelName + " " + load("SampleSceneTwo").maxTimeCounter);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            clearAllLevelStats();
            Debug.Log("Level Stats Cleared");
        }
        if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("Time Reduced by 1");
            buyUsingTime(1);
        }
        
        if (Input.GetKeyDown(KeyCode.A))
            //Debug.Log(totalTimeValue);
            
        if (Input.GetKeyDown(KeyCode.S))
            Debug.Log(Application.persistentDataPath);
        if (Input.GetKeyDown(KeyCode.D))
            Debug.Log(Application.persistentDataPath + directory);
        */
    }

}


[System.Serializable]
public struct levelStats
{
    public string levelName;
    public int totalDeathCounter;
    public float maxTimeCounter;
    public float devTime;
    public float minTime;
    //public bool levelBought;
    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }
    public levelStats(levelStats ls)
    {
        //copy stats
        levelName = ls.levelName;
        totalDeathCounter = ls.totalDeathCounter;
        maxTimeCounter = ls.maxTimeCounter;
        //levelBought = true;
        devTime = 0;
        minTime = 0;
    }

    public levelStats(string ln, int death, float time)
    {
        levelName = ln;
        totalDeathCounter = death;
        maxTimeCounter = time;
        //levelBought = levelbought;
        devTime = 0;
        minTime = 0;
    }

    public levelStats(string ln)
    {
        //level has been bought but not played
        //this level will be overwritten by levelInfo when level is actually played
        levelName = ln;
        totalDeathCounter = 0;
        maxTimeCounter = 0;
        devTime = 0;
        minTime = 0;
    }

    public bool levelFinished()
    {
        return (maxTimeCounter > minTime);
    }

    public bool levelFinishedGold()
    {
        return maxTimeCounter > devTime;
    }

    

    
}

