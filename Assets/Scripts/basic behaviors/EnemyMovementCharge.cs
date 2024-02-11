using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementCharge : MonoBehaviour
{
    //does not lerp to position but moves based on physics
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float waitTime = 2.5f;
    public bool doesSpawnRoutine;
    [SerializeField][Tooltip("Chance for this to charge to the player position")]
    [Range(0, 100)]
    int chargeToPlayer = 75;

    SpriteRenderer sr;

    void Start()
    {
        if (doesSpawnRoutine)
        {
            //animation
            spawnSequence();
        }
        else
        {
            StartCoroutine(move());
        }
    }

    void spawnSequence()
    {
        //animation
        StartCoroutine(move());
    }

    IEnumerator move()
    {
        int rnum = Random.Range(0,101);
        //if is on top of player, just move to random target position away from player

        Vector2 targetPos;
        if (rnum <= chargeToPlayer)
            targetPos = PlayerMovement.instance.transform.position;
        else
            targetPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));

        while (Vector2.Distance(transform.position, targetPos) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }


        yield return new WaitForSeconds(waitTime);
        //wait a while
        StartCoroutine(move());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health.instance.takeDamage(1);
        }
    }

}
