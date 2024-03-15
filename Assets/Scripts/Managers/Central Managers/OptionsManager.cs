using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class OptionsManager : MonoBehaviour
{
    public static OptionsManager instance;
    [SerializeField]
    private GameObject pauseMenuObject;
    private GameObject pauseMenuInstance;


    public static menuSettings savedSettings;

    bool isPaused = false;
    public bool GamePaused { get { return isPaused; } }
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;

            string dir = Application.persistentDataPath + directory;

            //if the directory does not exist
            if (!Directory.Exists(dir))
            {
                Debug.Log("Directory Created");
                Directory.CreateDirectory(dir);
                Debug.Log("Directory created");
                Save(new menuSettings(100));
            }
            else
            {
                loadOptions();
                //Debug.Log(savedSettings.musicVolume);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    private void deleteDirectory()
    {
        Directory.Delete(Application.persistentDataPath + directory);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            isPaused = !isPaused;
            Debug.Log("Pause status: " + isPaused);
            handlePause();
        }
    }
    public void resumeGame()
    {
        isPaused = !isPaused;
        handlePause();
    }

    void handlePause()
    {
        if (isPaused)
        {

            Transform parent = GameObject.Find("GameCanvas").transform;
            if (pauseMenuInstance != null)
            {
                pauseMenuInstance.SetActive(true);
            }
            else
            {
                pauseMenuInstance = Instantiate(pauseMenuObject, parent);

            }
            Time.timeScale = 0;

        }
        else if (!isPaused)
        {
            Time.timeScale = 1;

            pauseMenuInstance.SetActive(false);
        }


    }




    //save options 

    private static string settingsName = "PlayerSettings";
    private static string directory = "/SettingsData/";


    public static void Save(menuSettings potentialSettings)
    {
        instance.saveOptions(potentialSettings);
    }
    public void saveOptions(menuSettings potentialSettings)
    {
        Debug.Log("Settings Saving");
        Debug.Log(savedSettings.effectsVolume);
        savedSettings = potentialSettings;
        Debug.Log(savedSettings.effectsVolume);
        //we just overwrite whatever we have
        string dir = Application.persistentDataPath + directory;
        string json;

        string filePath = dir + settingsName + ".txt";
        json = JsonUtility.ToJson(savedSettings);
        File.WriteAllText(filePath, json);
        Debug.Log("Settings Saved");
    }
    public static menuSettings LoadOptions()
    {
        return instance.loadOptions();
    }

    public menuSettings loadOptions()
    {
        string dir = Application.persistentDataPath + directory;
        string filePath = dir + settingsName + ".txt";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            savedSettings = JsonUtility.FromJson<menuSettings>(json);
            return savedSettings;
            Debug.Log("Successfully loaded settings");
        }
        else
        {
            //save file does not exist - create new save file
            savedSettings = new menuSettings(100, 100, 100);
            Debug.Log("Settings not found, creating new settings");

            saveOptions(savedSettings);
            return new menuSettings();
        }
    }

}
