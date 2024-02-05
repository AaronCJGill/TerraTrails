using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementSpitter : MonoBehaviour
{
    [SerializeField]
    private float spitWaitTIme= 2.5f;
    public bool doesSpawnRoutine;
    [SerializeField]
    GameObject projectile;
    void Start()
    {
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

    void spawnSequence()
    {
        //animation
        StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        yield return new WaitForSeconds(spitWaitTIme);
        //wait a while
        Instantiate(projectile, transform.position, Quaternion.identity);

        
        StartCoroutine(shoot());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health.instance.takeDamage(1);
        }
    }


    //flies to random place or flies to player position

    //spits at player

    //repeat
}
