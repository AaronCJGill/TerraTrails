using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private Toggle fullscreentoggle;

    [SerializeField]
    private GameObject resumeButtonObject, exitButtonObject, settingsButtonObject, settingsObject;
    [SerializeField]
    private GameObject AudioTabParent, gameTabParent;
    [SerializeField]
    private GameObject gameTabButton;
    private enum tabActive
    {
        none,
        audio,
        gameSettings //resolution and such
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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            //dont allow game settings if in webgl
            gameTabButton.SetActive(false);
        }
        else
        {
            gameTabButton.SetActive(true);
        }
        //potentialSettings = OptionsManager.savedSettings;
        Debug.Log("Initializing settings");
        potentialSettings = new menuSettings(OptionsManager.savedSettings.masterVolume, OptionsManager.savedSettings.effectsVolume, OptionsManager.savedSettings.musicVolume);
        musicVolumeSlider.value = OptionsManager.savedSettings.musicVolume * 100;
        effectsVolumeSlider.value = OptionsManager.savedSettings.effectsVolume * 100;
        masterVolumeSlider.value = OptionsManager.savedSettings.masterVolume * 100;//master volume is just a multiplier for the volume other volume variables
        fullscreentoggle.isOn = (OptionsManager.savedSettings.resolutionSettings.screenMode == FullScreenMode.FullScreenWindow) ? true: false;

        //doing this quick and dirty, probably some convoluted ability to use a switch statement with additional parameters to get the value quickly but fuck it
        if (OptionsManager.savedSettings.resolutionSettings.currentRes == new Vector2Int(1280, 800))
        {
            resolutionDropdown.value = 0;
        }
        else if (OptionsManager.savedSettings.resolutionSettings.currentRes == new Vector2Int(1440, 900))
        {
            resolutionDropdown.value = 1;
        }
        else if (OptionsManager.savedSettings.resolutionSettings.currentRes == new Vector2Int(1680, 1050))
        {
            resolutionDropdown.value = 2;

        }
        else if (OptionsManager.savedSettings.resolutionSettings.currentRes == new Vector2Int(1920, 1200))
        {
            resolutionDropdown.value = 3;

        }
        else if (OptionsManager.savedSettings.resolutionSettings.currentRes == new Vector2Int(2560, 1600))
        {
            resolutionDropdown.value = 4;
        }

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
            Debug.Log("potential val" + (musicVolumeSlider.value * 100) + "  --- adjusted val " + (int)(musicVolumeSlider.value * 100));
            potentialSettings.effectsVolume = (int) (effectsVolumeSlider.value * 100 );
            potentialSettings.masterVolume = (int) (masterVolumeSlider.value * 100);
            //Debug.Log("audiotab");

        }

        if (activeTab == tabActive.gameSettings)
        {
            //handle checkbox settings
            FullScreenMode fsm;
            //fsm = (fullscreentoggle.isOn) ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            if (fullscreentoggle.isOn)
            {
                fsm = FullScreenMode.FullScreenWindow;
                //Debug.Log("ison");
            }
            else
            {
                fsm = FullScreenMode.Windowed;
                //Debug.Log("isoff");

            }
            //fsm = FullScreenMode.Windowed;


            ResolutionSettings rs;
            switch (resolutionDropdown.value)
            {
                /*
        WGXA,//1280	800
        WGXAPlus,//1440	900
        WSXGAPlus,//1680	1050
        WUXGA,//1920	1200
        WQXGA,//2560	1600 
                 */
                case 0:
                    rs = new ResolutionSettings(ResolutionSettings.resOptions.WGXA, fsm);
                    break;
                case 1:
                    rs = new ResolutionSettings(ResolutionSettings.resOptions.WGXAPlus, fsm);
                    break;
                case 2:
                    rs = new ResolutionSettings(ResolutionSettings.resOptions.WSXGAPlus, fsm);
                    break;
                case 3:
                    rs = new ResolutionSettings(ResolutionSettings.resOptions.WUXGA, fsm);
                    break;
                case 4:
                    rs = new ResolutionSettings(ResolutionSettings.resOptions.WQXGA, fsm);
                    break;
                default:
                    rs = new ResolutionSettings();
                    break;
            }
            //Debug.Log(rs.currentRes);
            potentialSettings.resolutionSettings = rs;
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
        //potentialSettings.effectsVolume /= 100;
        //potentialSettings.masterVolume /= 100;
        //potentialSettings.musicVolume /= 100;
        Debug.Log("Master vol after save: " + potentialSettings.masterVolume + " || Music: " +potentialSettings.musicVolume);

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
        SceneManager.LoadScene(2);
    }

    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    public void audioButton()
    {
        //show audio settings
        activeTab = tabActive.audio;
        AudioTabParent.SetActive(true);
        gameTabParent.SetActive(false);
    }
    public void gameButton()
    {
        activeTab = tabActive.gameSettings;
        gameTabParent.SetActive(true);
        AudioTabParent.SetActive(false);
    }

}

[System.Serializable]
public struct menuSettings
{
    public int masterVolume;
    public int effectsVolume;
    public int musicVolume;
    public ResolutionSettings resolutionSettings;
    public menuSettings(int mastVolume, int fxVolume, int musVolume)
    {
        masterVolume = Mathf.Min(mastVolume, 100);
        effectsVolume = Mathf.Min(fxVolume,100);
        musicVolume = Mathf.Min(musVolume, 100);
        resolutionSettings = new ResolutionSettings(ResolutionSettings.resOptions.WGXA);
    }
    public menuSettings(int mastVolume, int fxVolume, int musVolume, ResolutionSettings rs)
    {
        masterVolume = Mathf.Min(mastVolume, 100);
        effectsVolume = Mathf.Min(fxVolume, 100);
        musicVolume = Mathf.Min(musVolume, 100);
        resolutionSettings = rs;
    }
    public menuSettings(int x)
    {
        masterVolume = x;
        effectsVolume = x;
        musicVolume = 100;
        resolutionSettings = new ResolutionSettings(ResolutionSettings.resOptions.WGXA);
    }

}
[System.Serializable]
public struct ResolutionSettings
{
    public Vector2Int currentRes;
    public FullScreenMode screenMode;
    public ResolutionSettings(resOptions r = resOptions.WGXA)
    {
        currentRes = new Vector2Int(1280, 800);
        screenMode = FullScreenMode.Windowed;
    }
    public ResolutionSettings(resOptions r, FullScreenMode fsm)
    {
        switch (r)
        {
            case resOptions.WGXAPlus:
                currentRes = new Vector2Int(1440, 900);
                break;
            case resOptions.WSXGAPlus:
                currentRes = new Vector2Int(1680, 1050);
                break;
            case resOptions.WUXGA:
                currentRes = new Vector2Int(1920, 1200);
                break;
            case resOptions.WQXGA:
                currentRes = new Vector2Int(2560, 1600);
                break;
            case resOptions.itchBuild:
                currentRes = new Vector2Int(960, 600);
                break;
            default://defaults to WGXA
            case resOptions.WGXA:
                currentRes = new Vector2Int(1280, 800);
                break;
        }
        screenMode = fsm;
    }
    //public static Vector2Int WXGA = new Vector2Int(1280, 800);
    public enum resOptions
    {
        WGXA,//1280	800
        WGXAPlus,//1440	900
        WSXGAPlus,//1680	1050
        WUXGA,//1920	1200
        WQXGA,//2560	1600
        itchBuild //960  600

    }
}


