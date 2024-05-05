using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSfx : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("KAMIKAZE")]
    public AudioClip kaWalk;
    public AudioClip kaRun;
    public AudioClip kaExplode;

    [Header("SPITTER")]
    public AudioClip spLazer;

    [Header("TELEPORTER")]
    public AudioClip tePopup;
    public AudioClip tePlop;

    [Header("PLAYER")]
    public AudioClip plStep;

    [Header("CHARGER")]
    public AudioClip chrg;
    public AudioClip chStep;
    public AudioClip chBackStep;
    public AudioClip chStop;

    bool fireFx = true;
    bool active = true;

    AudioSource aud;
    PlayerMovement plM;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }


    //KAMIKAZE

    public void PlayKaWalk()
    {
        if (!GameManager.GameOver)
        {

        }

        aud.pitch = Random.Range(.8f, 1.3f);
        aud.PlayOneShot(kaWalk, .5f);
    }
    public void PlayKaRun()
    {
        //aud.pitch = Random.Range(.7f, 1f);
        aud.PlayOneShot(kaRun, 1f);
    }
    public void PlayKaExplode()
    {
        aud.pitch = 1;
        aud.PlayOneShot(kaExplode, 1f);
    }

    //SPITTER

    public void PlaySpLazer()
    { 

        if (fireFx)
        {
            aud.PlayOneShot(spLazer, 1f);
        }
    }

    //TELEPORT

    public void PlayTePopup()
    {
        aud.pitch = Random.Range(1, 1);
        aud.PlayOneShot(tePopup, .9f);
    }
    public void PlayTePlop()
    {
        aud.pitch = Random.Range(1, 1); 
        aud.PlayOneShot(tePlop, .9f);
    }

    //PLAYER

    public void PlayPlStep()
    {
        aud.pitch = Random.Range(2f, 3f);
        aud.volume = 1;
        aud.PlayOneShot(plStep, 1F);
    }

    //CHARGER

    public void PlaychChrg()
    {
        aud.pitch = Random.Range(1,1);
        aud.volume = 1;
        aud.PlayOneShot(chrg, 1f);
    }
    public void PlaychStep()
    {
        if (!GameManager.GameOver)
        {
            aud.pitch = 1;
            aud.volume = .4f;
            aud.PlayOneShot(chStep, 1F);
        }
    }
    public void PlaychBackStep()
    {
        if (!GameManager.GameOver)
        {
            aud.volume = .4f;
            aud.pitch = 1;
            aud.PlayOneShot(chBackStep, 1);
        }
    }
    public void PlaychStop()
    {
        aud.volume = 1;
        aud.pitch = 2;
        aud.PlayOneShot(chStop, 1);
    }
}
