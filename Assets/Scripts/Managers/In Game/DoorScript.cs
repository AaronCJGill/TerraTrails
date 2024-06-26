using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class DoorScript : MonoBehaviour
{
    [SerializeField]
    GameObject sprite;
    [SerializeField]
    BoxCollider2D bxCol;
    public static DoorScript instance;
    [SerializeField]
    private AudioSource _as;
    bool levelDone = false;
    private void Awake()
    {
        bxCol = GetComponent<BoxCollider2D>();
        
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            _as = GetComponent<AudioSource>();
        }
    }
    private void Start()
    {
        sprite.SetActive(false);
        bxCol.enabled = false;
        bxCol.isTrigger = true;
    }

    //possibly randomized door locations
    void Init()
    {
        //called when randomized - currently not implemented
    }
    
    public static void levelOver()
    {
        //allow door to be spawned in 
        //sprite child object is now loaded, box collider is active
        instance._as.volume = CentralAudioManager.instance.sfxVolume;
        if (!instance.levelDone)
        {
            instance.levelDone = true;
            instance._as.pitch = 1;
            instance._as.Play();
        }
        //instance._as.Play();
        instance.bxCol.enabled = true;
        instance.sprite.SetActive(true);
    }
    //when the player is touched, move to the next level, maybe play an animation first

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //move down one level
            GameManager.instance.GameOverSequence();
            SpawnManager.levelEnd();
        }
    }
}
