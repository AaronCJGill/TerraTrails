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

    AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
    }



    //KAMIKAZE

    public void PlayKaWalk()
    {
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
        //aud.pitch = Random.Range(.7f, 1f);
        aud.PlayOneShot(spLazer, 1f);
    }

    //TELEPORT

    public void PlayTePopup()
    {
        aud.pitch = Random.Range(.8f, 1f);
        aud.PlayOneShot(tePopup, .6f);
    }
    public void PlayTePlop()
    {
        aud.pitch = Random.Range(.7f, 1f);
        aud.PlayOneShot(tePlop, .7f);
    }

    //PLAYER

    public void PlayPlStep()
    {
        aud.pitch = Random.Range(.7f, .9f);
        aud.volume = .8f;
        aud.PlayOneShot(plStep, 1F);
    }

    //CHARGER

    public void PlaychChrg()
    {
        aud.pitch = Random.Range(0.9f, 1.1f);
        aud.PlayOneShot(chrg,1F);
    }
    public void PlaychStep()
    {
        aud.pitch = Random.Range(.9f,1f);
        aud.volume = 1f;
        aud.PlayOneShot(chStop, 1F);
    }
    public void PlaychBackStep()
    {
        aud.pitch = Random.Range(1.1f, 1.2f);
        aud.volume = .6f;
        aud.PlayOneShot(chStop, 1F);
    }
    public void PlaychStop()
    {
        aud.volume = 1f;
        aud.pitch = 2;
        aud.PlayOneShot(chStop, 1F);
    }
}
