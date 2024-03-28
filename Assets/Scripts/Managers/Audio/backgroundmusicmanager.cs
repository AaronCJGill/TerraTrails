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
        Level
    }

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
        _as.clip = CentralAudioManager.retrieveSound(SoundFiles.backgroundMusic);
        _as.Play();
    }

    

    public static void changeBackgroundMusic(typeOfMusic tom)
    {
        //change the music to whatever is requested
    }


    //CHANGE MUSIC USING SMOOTHSTEP TO THE new music type chosen. SMOOTHSTEP VOLUME TO 0 CHANGE AT 0 AND SWITCH TO NEW ONE STEPPING TO WHATEVER IS THE CHOSEN SOUND SETTING

    //SOUND SETTINGS ARE OFTEN LIVE - CALL SOUND CHANGE SETTING ON UPDATE IN MENU OBJECT SCRIPT

    // Update is called once per frame
    void Update()
    {
        _as.volume = CentralAudioManager.instance.bgmVolume;

    }




}
