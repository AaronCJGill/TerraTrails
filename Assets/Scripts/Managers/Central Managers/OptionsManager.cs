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
                Debug.Log("Settings Directory Created");
                Directory.CreateDirectory(dir);
                ResolutionSettings rs; //= new ResolutionSettings(ResolutionSettings.resOptions.WGXA, FullScreenMode.Windowed);
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    rs = new ResolutionSettings(ResolutionSettings.resOptions.itchBuild, FullScreenMode.Windowed);
                }
                else
                {
                    rs = new ResolutionSettings(ResolutionSettings.resOptions.WGXA, FullScreenMode.Windowed);
                }
                menuSettings ms = new menuSettings(100, 100, 100, rs);
                //ms.resolutionSettings = rs;
                Save(ms);
            }
            else
            {
                loadOptions();
                //Debug.Log("Loading saved settings");
            }
        }

        DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
        Save(savedSettings);
    }
    public void deleteDirectory()
    {
        Directory.Delete(Application.persistentDataPath + directory);
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 2)
        {
            if (Health.instance != null && !Health.isDead)
            {
                
                isPaused = !isPaused;
                Debug.Log("Pause status: " + isPaused);
                handlePause();
            }
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

        //handle sound saving
        CentralAudioManager.updateSoundSettings();

        //handle resolution
        ResolutionSettings rs = potentialSettings.resolutionSettings;
        //Debug.Log("RS Fullscreen mode : " + rs.screenMode);
        Screen.SetResolution(rs.currentRes.x, rs.currentRes.y, rs.screenMode);
        //Debug.Log("RS Fullscreen mode : " + rs.screenMode + " - fsm " + Screen.fullScreenMode);

        instance.updateSettings();
    }

    public void updateSettings()
    {

        //handle sound saving
        CentralAudioManager.updateSoundSettings();
        ResolutionSettings rs = savedSettings.resolutionSettings;
        //handle resolution
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            rs = new ResolutionSettings(ResolutionSettings.resOptions.itchBuild, FullScreenMode.Windowed);

        }
        else
        {
            rs = savedSettings.resolutionSettings;
        }
        //Debug.Log(rs.currentRes + " - " +rs.screenMode);
        Screen.SetResolution(rs.currentRes.x, rs.currentRes.y, rs.screenMode);
    }

    public void saveOptions(menuSettings potentialSettings)
    {
        //Debug.Log("Settings Saving");
        //Debug.Log(savedSettings.effectsVolume);
        savedSettings = potentialSettings;
        //Debug.Log(savedSettings.resolutionSettings.currentRes);
        //we just overwrite whatever we have
        string dir = Application.persistentDataPath + directory;
        string json;

        string filePath = dir + settingsName + ".txt";
        json = JsonUtility.ToJson(savedSettings);
        File.WriteAllText(filePath, json);
        //Debug.Log("Settings Saved");
        updateSettings();
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

            Screen.SetResolution(savedSettings.resolutionSettings.currentRes.x, savedSettings.resolutionSettings.currentRes.y, savedSettings.resolutionSettings.screenMode);
            CentralAudioManager.updateSoundSettings();
            return savedSettings;
            //Debug.Log("Successfully loaded settings");
        }
        else
        {
            //save file does not exist - create new save file
            //savedSettings = new menuSettings(100, 100, 100);
            //Debug.Log("Settings not found, creating new settings");
            ResolutionSettings rs;
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                rs = new ResolutionSettings(ResolutionSettings.resOptions.itchBuild, FullScreenMode.Windowed);
            }
            else
            {
                rs = new ResolutionSettings(ResolutionSettings.resOptions.WGXA, FullScreenMode.Windowed);
            }
            menuSettings ms = new menuSettings(100, 100, 100, rs);
            //ms.resolutionSettings = rs;
            //Save(ms);
            savedSettings = ms;
            //updateSettings();
            Save(ms);
            return ms;
        }
    }
}
