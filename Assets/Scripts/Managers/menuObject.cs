using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class menuObject : MonoBehaviour
{
    

    private menuSettings potentialSettings;
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private Slider effectsVolumeSlider;
    [SerializeField]
    private Slider masterVolumeSlider;

    [SerializeField]
    private GameObject resumeButtonObject, exitButtonObject, settingsButtonObject, settingsObject;
    private List<GameObject> AudioTabObjects = new List<GameObject>();

    private enum tabActive
    {
        none,
        audio,
    }
    private tabActive activeTab = tabActive.audio;

    public static menuObject instance;
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
        //need to find a way to set this active
    }

    // Start is called before the first frame update
    void Start()
    {
        initSettings();
    }
    void initSettings()
    {
        //potentialSettings = OptionsManager.savedSettings;
        Debug.Log("Initializing settings");
        potentialSettings = new menuSettings(OptionsManager.savedSettings.masterVolume, OptionsManager.savedSettings.effectsVolume, OptionsManager.savedSettings.musicVolume);
        musicVolumeSlider.value = OptionsManager.savedSettings.musicVolume * 100;
        effectsVolumeSlider.value = OptionsManager.savedSettings.effectsVolume * 100;
        masterVolumeSlider.value = OptionsManager.savedSettings.masterVolume * 100;//master volume is just a multiplier for the volume other volume variables
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cancelSettings();
        }


        //TODO - simplify to switch statement
        if (activeTab == tabActive.audio)
        {
            potentialSettings.musicVolume = (int) (musicVolumeSlider.value * 100);
            //Debug.Log("potential val" + (musicVolumeSlider.value * 100) + "  --- adjusted val " + (int)(musicVolumeSlider.value * 100));
            potentialSettings.effectsVolume = (int) (effectsVolumeSlider.value * 100 );
            potentialSettings.masterVolume = (int) (masterVolumeSlider.value * 100);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(potentialSettings);
        }
        //potentialSettings.musicVolume = Mathf.Ceil(volumeSlider.value);
    }

    public void deactivateSettingsPage()
    {
        settingsObject.SetActive(false);
    }
    public void activateSettingsPage()
    {
        settingsObject.SetActive(true);
    }

    public void cancelSettings()
    {
        //close page
        deactivateSettingsPage();
        initSettings();
    }

    public void saveSettings()
    {
        OptionsManager.Save(potentialSettings);
        deactivateSettingsPage();
        //maybe resume as well
    }
    public void resume()
    {
        OptionsManager.instance.resumeGame();
    }
    public void optionsButton()
    {
        activateSettingsPage();
    }

    public void quitButton()
    {
        SceneManager.LoadScene(1);
    }

    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    public void audioButton()
    {
        //show audio settings
        foreach (var item in AudioTabObjects)
        {
            //deactivate each individually

            //or we can just all objects be parented by the tab 
        }
    }


}


public struct menuSettings
{
    public int masterVolume;
    public int effectsVolume;
    public int musicVolume;

    public menuSettings(int mastVolume, int fxVolume, int musVolume)
    {
        masterVolume = Mathf.Min(mastVolume, 100);
        effectsVolume = Mathf.Min(fxVolume,100);
        musicVolume = Mathf.Min(musVolume, 100);
    }

    public menuSettings(int x)
    {
        masterVolume = x;
        effectsVolume = x;
        musicVolume = 100;
    }

}



