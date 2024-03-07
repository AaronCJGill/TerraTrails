using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralAudioManager : MonoBehaviour
{
    //has the files for all of the possible sounds in the game

    //simple call this object with a sound 

    public static CentralAudioManager instance;

    [SerializeField]
    private AudioClip backgroundMusic;
    [SerializeField]
    private AudioClip chargerSFX;
    [SerializeField]
    private AudioClip shootingSFX;
    [SerializeField]
    private AudioClip parrySFX;
    [SerializeField]
    private AudioClip dashSFX;
    [SerializeField]
    private AudioClip teleportSFX;
    [SerializeField]
    private AudioClip playerWalkSFX;
    [SerializeField]
    private AudioClip playerhitSFX;


    public float sfxVolume;
    public float bgmVolume;

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
    }

    public static AudioClip retrieveSound(SoundFiles wantedSound)
    {
        switch (wantedSound)
        {
            case SoundFiles.backgroundMusic:
                return instance.backgroundMusic;
            case SoundFiles.chargerSFX:
                return instance.chargerSFX;
            case SoundFiles.shootingSFX:
                return instance.shootingSFX;
            case SoundFiles.parrySFX:
                return instance.parrySFX;
            case SoundFiles.dashSFX:
                return instance.dashSFX;
            case SoundFiles.playerHitSFX:
                return instance.playerhitSFX;
            case SoundFiles.playerWalkSFX:
                return instance.playerWalkSFX;
            case SoundFiles.teleportSFX:
                return instance.teleportSFX;
            default:
                return null;
        }
    }

    private void Update()
    {
        //volume manager
        bgmVolume = (OptionsManager.savedSettings.musicVolume * OptionsManager.savedSettings.masterVolume )/ 100;
        sfxVolume = (OptionsManager.savedSettings.effectsVolume * OptionsManager.savedSettings.masterVolume) / 100;
        //Debug.Log(OptionsManager.savedSettings.musicVolume + " " + OptionsManager.savedSettings.effectsVolume + " " +sfxVolume + " " + bgmVolume) ;
    }


}
public enum SoundFiles
{
    none,
    backgroundMusic,
    chargerSFX,
    shootingSFX,
    parrySFX,
    dashSFX,
    playerWalkSFX,
    playerHitSFX,
    teleportSFX,

}