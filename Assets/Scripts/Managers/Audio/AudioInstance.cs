using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstance : MonoBehaviour
{
    /// <summary>
    /// this is only used for enemies and other things that make simple sounds. 
    /// this would be referenced by another file to call it to simply playsound
    /// </summary>
    [SerializeField]
    private SoundFiles sound;
    [Tooltip("Not necessary unless this plays more than one sound. Sound order is Shoot, teleport, charge")][SerializeField]
    private SoundFiles secondSound;
    private AudioClip soundToPlay;
    private AudioSource _as;

    private void Start()
    {
        if (TryGetComponent<AudioSource>(out AudioSource AS))
        {
            _as = AS;
        }
        else
        {
            _as = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }
        _as.volume = (OptionsManager.savedSettings.effectsVolume * OptionsManager.savedSettings.masterVolume);
        //Debug.Log(_as.volume + " VOLUME -- supposed to be " +(OptionsManager.savedSettings.effectsVolume * OptionsManager.savedSettings.masterVolume));
        soundToPlay = CentralAudioManager.retrieveSound(sound);
        _as.playOnAwake = false;
    }
    private void Update()
    {
        _as.volume = CentralAudioManager.instance.sfxVolume;
    }
    public void playSound()
    {
        _as.clip = CentralAudioManager.retrieveSound(sound);
        _as.Play();
    }

    public void playFirstSound()
    {
        _as.clip = CentralAudioManager.retrieveSound(sound);
        _as.Play();
    }

    public void playSecondSound()
    {
        _as.clip = CentralAudioManager.retrieveSound(secondSound);
        _as.Play();
    }



}
