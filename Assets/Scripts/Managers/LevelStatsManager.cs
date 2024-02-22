using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelStatsManager : MonoBehaviour
{
    public static string directory = "/SaveData/";
    public static LevelStatsManager instance;
    public static int totalDeathCount;
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

    }

    public void increaseTotalDeaths()
    {

    }
    public void increaseTotalTime(float oldTime, float newTime)
    {
        totalTimeValue = totalTimeValue + (newTime - oldTime);
    }


    //each level has its own json file with its stats
    public void save(levelStats ls)
    {
        string dir = Application.persistentDataPath + directory;
        string json = JsonUtility.ToJson(ls);
        File.WriteAllText(dir + ls.levelName + ".txt", json);
        
        //get json and save it if it does not
    }


    bool checkIfDirectoryExists(string fullPath)
    {
        
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
    
    
    
    //returns the level based on levelname
    public levelStats load(string levelName)
    {
        string dir = Application.persistentDataPath + directory;
        string filePath = dir + levelName + ".txt";
        
        if (checkIfDirectoryExists(levelName))
        {
            string json = File.ReadAllText(filePath);
            levelStats ls = JsonUtility.FromJson<levelStats>(json);
            Debug.Log("Successfully loaded map");
            return ls;
        }
        else
        {
            Debug.Log("Could not load level - New levelStats Made");
            return new levelStats();
        }
    }


    //maybe clear individual level but not sure? how would this work with time currency?
    public void clearAllLevelStats()
    {
        //for each level, delete all data
        string[] allFileList = Directory.GetFiles(directory);
        foreach (string item in allFileList)
        {
            File.Delete(directory + item);
        }
        //reset global stats

    }

    public void deleteLevelStats(levelStats ls)
    {
        //destroys the levels stats
        //subtracts amounts from the global file

        if (true)
        {

        }
    }


    public void saveGlobalStats()
    {
        
    }

    public void addTimeToStats()
    {
        //takes in the new time, 
    }

    public void buyTime(float cost)
    {
        
    }
}


[System.Serializable]
public struct levelStats
{
    public string levelName;
    public int totalDeathCounter;
    public float maxTimeCounter;
    public string saveToString()
    {
        return JsonUtility.ToJson(this);
    }
}

