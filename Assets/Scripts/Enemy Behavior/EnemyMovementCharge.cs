using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementCharge : MonoBehaviour, enemyDestroy
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
    [Header("Pathfinding Settings")]
    [SerializeField]
    private Transform storedTransform;
    [SerializeField]
    Pathfinding.Seeker seeker;
    [SerializeField]
    Pathfinding.AIPath pathing;
    [SerializeField]
    Pathfinding.AIDestinationSetter destinationSetter;
    [SerializeField][Tooltip("Amount of time this can spend charging. Either spends this much time charging or it hits its point")]
    float chargingTime = 10;
    [SerializeField][Tooltip("Keep this as 10 if you want it to stay the default value.")]
    float chargingAcceleration = 1000;
    [SerializeField]
    [Tooltip("Keep in mind when using this does not factor in the 1.5 second recharge time the movement timer has. Add 1.5 to this number to get real time until it charges after a stun")]
    float stunTime = 3f;
    [SerializeField]
    float stopDistance = 0.3f;
    [SerializeField]
    float picknextwaypointdistance = 0.8f;
    void Start()
    {
        seeker = GetComponent<Pathfinding.Seeker>();
        pathing = GetComponent<Pathfinding.AIPath>();
        pathing.maxSpeed = speed;
        pathing.slowdownDistance = stopDistance;
        pathing.pickNextWaypointDist = picknextwaypointdistance; 
        //pathing.maxa
        destinationSetter = GetComponent<Pathfinding.AIDestinationSetter>();
        storedTransform = new GameObject("Charger " + "Target -- "  + name).transform;
        pathing.canMove = false;
        storedTransform.position = PlayerMovement.instance.Position;
        destinationSetter.target = storedTransform;
        pathing.maxAcceleration = chargingAcceleration;
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

        float chargingTimer = 0;

        if (PlayerMovement.instance.Position.x > transform.position.x)
        {
            //target is to the right
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        yield return new WaitForSeconds(1.5f);
        int rnum = Random.Range(0,101);
        //if is on top of player, just move to random target position away from player
        //anim.ResetTrigger("chargeUp");
        //anim.SetTrigger("charge");
        
        Vector2 targetPos;
        if (rnum <= chargeToPlayer && Vector2.Distance(transform.position, PlayerMovement.instance.transform.position) > 1)
            targetPos = PlayerMovement.instance.transform.position;
        else
            targetPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        /*
        if (targetPos.x > transform.position.x)
        {
            //target is to the right
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
        */
        float targetDeviationRangeX = Random.Range(-1,1);
        float targetDeviationRangeY = Random.Range(-1,1);
        //deviates from the 
        if (targetPos.x + targetDeviationRangeX > LevelBoundary.BottomRightBound.X || targetPos.x + targetDeviationRangeX < LevelBoundary.leftTopBound.X)
        {
            targetDeviationRangeX = 0;
        }
        if (targetPos.y + targetDeviationRangeY > LevelBoundary.leftTopBound.Y || targetPos.y + targetDeviationRangeY < LevelBoundary.BottomRightBound.Y)
        {
            targetDeviationRangeY = 0;
        }
        targetPos.x += targetDeviationRangeX;
        targetPos.y += targetDeviationRangeY;
        storedTransform.position = targetPos;
        while (Vector2.Distance(transform.position, targetPos) > stopDistance && chargingTimer < chargingTime)
        {
            //transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            pathing.canMove = true;
            Debug.Log(chargingTimer);
            chargingTimer += Time.deltaTime;
            yield return null;
        }
        anim.ResetTrigger("charge");
        anim.SetTrigger("idle");
        pathing.canMove = false;
        yield return new WaitForSeconds(waitTime + Random.Range(-chargeRandomization, chargeRandomization));
        //anim.ResetTrigger("charge");
        //wait a while
        StartCoroutine(move());
    }
    private void Update()
    {
        if (storedTransform.position.x > transform.position.x)
        {
            //target is to the right
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

    }

    public void getStunned()
    {
        StopAllCoroutines();
        anim.ResetTrigger("charge");
        anim.SetTrigger("idle");
        Debug.Log("Stopping coroutine");
        StartCoroutine(stunRoutine());
    }
    
    IEnumerator stunRoutine()
    {
        pathing.canMove = false;
        Debug.Log("cant move");
        yield return new WaitForSeconds(stunTime);
        Debug.Log("can move stun over");
        StartCoroutine(move());
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
