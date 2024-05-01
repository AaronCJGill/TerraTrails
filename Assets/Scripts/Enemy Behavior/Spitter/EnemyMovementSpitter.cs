using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementSpitter : MonoBehaviour, enemyDestroy
{
    [SerializeField]
    private float spitWaitTIme= 2.5f;
    public bool doesSpawnRoutine;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    AudioInstance audioinstance;

    [SerializeField]
    Animator anim;
    [SerializeField]
    SpriteRenderer sr;
    void Start()
    {
        audioinstance = GetComponent<AudioInstance>();
        if(audioinstance)
            Debug.Log("Audio not found");
        if (doesSpawnRoutine)
        {
            //animation
            spawnSequence();
        }
        else
        {
            StartCoroutine(shoot());
        }
    }
    private void Update()
    {
        if (transform.position.x < PlayerMovement.instance.transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
    void spawnSequence()
    {
        //animation
        StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        audioinstance.playFirstSound();
        yield return new WaitForSeconds(spitWaitTIme - 0.83f);
        //time it so that the animation triggers at the correct moment
        anim.SetTrigger("action");
        yield return new WaitForSeconds(0.915f);
        //wait a while
        Instantiate(projectile, transform.position, Quaternion.identity);
        audioinstance.playSound();


        StartCoroutine(shoot());
        anim.ResetTrigger("action");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health.instance.takeDamage(1);
        }
    }

    public void levelEnd()
    {
        StopAllCoroutines();
    }
}
