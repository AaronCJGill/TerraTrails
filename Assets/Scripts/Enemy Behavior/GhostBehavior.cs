using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour, enemyDestroy
{
    //possibly make this chase the player at 10 speed, for around 2 seconds

    [SerializeField]
    private float speed = 10;
    [SerializeField]
    public bool doesSpawnRoutine;
    bool canmove = true;
    SpriteRenderer sr;

    bool playerIsNotDead;
    Vector2 targetPosition;

    void Start()
    {
        if (doesSpawnRoutine)
        {
            //animation
            spawnSequence();
        }
        else
        {
            //StartCoroutine(move());
        }
    }

    void spawnSequence()
    {
        //StartCoroutine(movement(startupWaitTime));
    }


    private void Update()
    {
        if (Health.isDead)
        {
            afterDeathWander();
        }
        else
        {
            //player alive, chase 
            targetPosition = PlayerMovement.instance.Position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            //maybe implement animation change for when ghost is close to player
        }
    }

    void afterDeathWander()
    {
        //choose random position
        if (Vector2.Distance(transform.position, targetPosition) < 4)
        {
            StartCoroutine(waitAround());
            targetPosition = usefulFunctions.positioning.getRandomSpawnPoint();
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    IEnumerator waitAround()
    {
        canmove = false;
        yield return new WaitForSeconds(Random.Range(0,3));
        canmove = true;
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
