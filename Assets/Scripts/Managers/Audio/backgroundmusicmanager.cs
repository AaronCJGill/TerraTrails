using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundmusicmanager : MonoBehaviour
{
    //i imagine there will be several types of background music - one for map, one for main menu, one for levels
    //multiple audiosources attached to this object. just set one to active
    public enum typeOfMusic
    {
        Map,
        MainMenu,
        introcutscene,
        Level,
        Gameover
    }

    public AudioClip mmMusic, introMusic,MapMusic, industrialMusic, organicLevel, gameoverMus;

    public static backgroundmusicmanager instance;
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    AudioSource _as;

    void Start()
    {
        //get access to the bgm from the centralaudiomanager
        _as = GetComponent<AudioSource>();
        _as.playOnAwake = true;
        _as.loop = true;
        //_as.clip = CentralAudioManager.retrieveSound(SoundFiles.backgroundMusic);
        //_as.Play();
        _as.volume = CentralAudioManager.instance.bgmVolume;
        _as.clip = mmMusic;
        _as.Play();
    }



    public enum levtype
    {
        mainmenu,
        intro,
        levelselect,
        industrial,
        organic,
        gameover,
        passLevel,
        none
    }
    public void changeBackgroundMusic(levtype type)
    {
        //this is called from helper backgroundmusicinfo script
        
        if (CentralAudioManager.instance != null)
        {
            Debug.Log("Getting this");
            _as.volume = CentralAudioManager.instance.bgmVolume;
        }
        //change the music to whatever is requested
        switch (type)
        {
            case levtype.mainmenu:
                _as.clip = mmMusic;
                _as.Play();
                break;
            case levtype.intro:
                _as.clip = introMusic;
                _as.Play();
                break;
            case levtype.levelselect:
                _as.clip = MapMusic;
                _as.Play();
                break;
            case levtype.industrial:
                _as.clip = industrialMusic;
                _as.Play();
                break;
            case levtype.organic:
                _as.clip = organicLevel;
                _as.Play();
                break;
            case levtype.gameover:
                _as.clip = gameoverMus;
                _as.Play();
                break;
            case levtype.passLevel:
                _as.clip = introMusic;
                _as.Play();
                break;
            case levtype.none:
                _as.Stop();
                break;
            default:
                _as.clip = industrialMusic;
                _as.Play();
                break;
        }
    }

    //CHANGE MUSIC USING SMOOTHSTEP TO THE new music type chosen. SMOOTHSTEP VOLUME TO 0 CHANGE AT 0 AND SWITCH TO NEW ONE STEPPING TO WHATEVER IS THE CHOSEN SOUND SETTING

    //SOUND SETTINGS ARE OFTEN LIVE - CALL SOUND CHANGE SETTING ON UPDATE IN MENU OBJECT SCRIPT

    // Update is called once per frame
    void Update()
    {
        _as.volume = CentralAudioManager.instance.bgmVolume;
        //_as.volume = 0;

    }




}
