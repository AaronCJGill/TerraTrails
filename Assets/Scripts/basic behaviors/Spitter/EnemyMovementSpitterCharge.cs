using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementSpitterCharge : MonoBehaviour
{
    //does not lerp to position but moves based on physics
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float moveWaitTime = 2.5f;
    [SerializeField]
    private float spitWaitTime = 1f;
    public bool doesSpawnRoutine;
    [SerializeField]
    [Tooltip("Chance for this to charge to the player position")]
    [Range(0, 100)]
    int chargeToPlayer = 75;
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
        int rnum = Random.Range(0, 101);
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
        yield return new WaitForSeconds(spitWaitTime);
        Instantiate(projectile, transform.position, Quaternion.identity);


        yield return new WaitForSeconds(moveWaitTime);
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
