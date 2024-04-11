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
    [Header("Randomization Settings")]
    [SerializeField]
    float chargeRandomization = 0.5f;
    [SerializeField]
    SpriteRenderer sr;
    [SerializeField]
    float startupWaitTime = 1f;
    [SerializeField]
    private Animator anim;//just two animations, charges and charging up


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
        StartCoroutine(move(startupWaitTime));
    }

    IEnumerator move(float startWaitTime = 0)
    {
        anim.ResetTrigger("idle");
        anim.SetTrigger("charge");

        //anim.SetTrigger("chargeUp");
        //yield return new WaitForSeconds(startWaitTime);
        yield return new WaitForSeconds(1.1f);
        int rnum = Random.Range(0,101);
        //if is on top of player, just move to random target position away from player
        //anim.ResetTrigger("chargeUp");
        //anim.SetTrigger("charge");

        Vector2 targetPos;
        if (rnum <= chargeToPlayer && Vector2.Distance(transform.position, PlayerMovement.instance.transform.position) > 1)
            targetPos = PlayerMovement.instance.transform.position;
        else
            targetPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));

        if (targetPos.x > transform.position.x)
        {
            //target is to the right
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        float targetDeviationRangeX = Random.Range(-1,1);
        float targetDeviationRangeY = Random.Range(-1,1);
        targetPos.x += targetDeviationRangeX;
        targetPos.y += targetDeviationRangeY;

        while (Vector2.Distance(transform.position, targetPos) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        anim.ResetTrigger("charge");
        anim.SetTrigger("idle");

        yield return new WaitForSeconds(waitTime + Random.Range(-chargeRandomization, chargeRandomization));

        //anim.ResetTrigger("charge");
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
