using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kamikazeBehavior : MonoBehaviour, enemyDestroy
{
    float speed = 10;
    [SerializeField]
    float fastSpeed = 10;
    [SerializeField]
    float wanderSpeed = 4;

    [SerializeField]
    float detectionRange = 10;
    [SerializeField][Tooltip("how large this explosion is")]
    float explosionSize = 5;
    [SerializeField][Tooltip("How far this should be away to start exploding")]
    float explosionDistance = 2;
    [SerializeField]
    detectionCircle dc;
    [SerializeField]
    GameObject explosionPrefab;
    Rigidbody2D rb;
    [SerializeField]
    Animator anim;

    Vector2 lastPos;

    enum state
    {
        dormant,//stays in place, waiting to detect player
        cooldown,//stays in place, unable to detect player or move
        chasing,//actively chasing the player
        exploding
    }
    state currentState;
    [SerializeField]
    float cooldownPeriod = 5;
    float cooldownTimer = 0;
    [SerializeField]
    float explodeWaitTime = 5;
    float explodeWaitTimer = 0;
    float explosionTime = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        switchState(state.dormant);
        dc.init(detectionRange, this);
        rb = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentState);
        switch (currentState)
        {
            case state.dormant:
                dormantBehavior();
                break;
            case state.cooldown:
                cooldownBehavior();
                break;
            case state.chasing:
                chaseBehavior();
                break;
            case state.exploding:
                explodingBehavior();
                break;
            default:
                break;
        }
        //Debug.Log(rb.velocity.magnitude + " " + anim.GetFloat("Vel"));
        //anim.SetFloat("Vel",rb.velocity.magnitude);
    }
    private void LateUpdate()
    {
        lastPos = transform.position;
    }
    void switchState(state s)
    {
        currentState = s;
        switch (s)
        {
            case state.dormant:
                dormantStart();
                break;
            case state.cooldown:
                cooldownStart();
                break;
            case state.chasing:
                chaseStart();
                break;
            case state.exploding:
                explodingStart();
                break;
            default:
                break;
        }
    }

    void dormantStart()
    {
        //waits in place - does nothing
        dc.dormantPeriod();
        speed = wanderSpeed;
    }

    Vector2 randomDestinationPoint;
    float pause;
    float pTime;
    void dormantBehavior()
    {

        //wait for the dc to just give input
        //just walk around randomly

        if (Vector2.Distance(transform.position, randomDestinationPoint) > 1)
        {
            //if we are faw away from distance point, just move
            transform.position = Vector2.MoveTowards(transform.position, randomDestinationPoint, speed * Time.deltaTime);
            anim.SetFloat("Vel", Vector2.MoveTowards(transform.position, randomDestinationPoint, speed * Time.deltaTime).magnitude);

            pTime = Random.Range(3, 6);//generate random value for pausetime
        }
        else //we are close to the point and should pause
        {
            pause += Time.deltaTime;
            //we do not move, but only add to pause time
            if (pause > pTime)
            {
                Vector2 potentialSpot = usefulFunctions.positioning.getRandomSpawnPoint();

                if (Vector2.Distance(transform.position, potentialSpot) < 6)
                {
                    randomDestinationPoint = potentialSpot;
                    pause = 0;
                }
            }
            anim.SetFloat("Vel", 0);

        }

        /*
        if (pause < pTime)
        {
            //paused
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, randomDestinationPoint, speed * Time.deltaTime);
            //Debug.Log();
        }
        */

    }
    void chaseStart()
    {
        //Debug.Log("Chase starting");
        speed = fastSpeed;
    }
    void chaseBehavior()
    {
        //move towards the player constantly
        if (!Health.isDead)
        {
            //only way it stops chasing is by getting close to player
            if (Vector2.Distance(transform.position, PlayerMovement.instance.Position) < explosionDistance)//if close enough stop chasing and explode next to player
            {
                //explode
                switchState(state.exploding);
                anim.SetTrigger("chargeup");
            }
            else
            {
                //move towards the player if we are not close enough to start exploding
                transform.position = Vector2.MoveTowards(transform.position, PlayerMovement.instance.Position, speed * Time.deltaTime);
                anim.SetFloat("Vel", Vector2.MoveTowards(transform.position, PlayerMovement.instance.Position, speed * Time.deltaTime).magnitude);
            }
        }
    }
    void explodingStart()
    {
        randomDestinationPoint = transform.position;
        //stay in place,
        //change anim
        Invoke("spawnExplosion", 0.6f);
    }
    void spawnExplosion()
    {
        GameObject g = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        g.GetComponent<kamikazeExplosion>().init(explosionSize, explosionTime);
    }

    void explodingBehavior()
    {
        explodeWaitTimer += Time.deltaTime;

        if (explodeWaitTime >= explodeWaitTimer)
        {
            //explosion has already been spawned, just wait and possibly play an animation
            switchState(state.cooldown);
            explodeWaitTimer = 0;
        }
    }
    void cooldownStart()
    {
        //probably do cooldown anim, ideally a trigger
        anim.SetFloat("Vel", 0);
        //reactivate dc
        anim.ResetTrigger("chargeup");
        anim.SetBool("recharging", true);
        //do cooldown anim
        dc.cooldownPeriod(cooldownPeriod);
    }
    void cooldownBehavior()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= cooldownPeriod -1.5f)//subtract the time needed for cooldown anim
        {
            anim.SetBool("recharging", false);
        }
        if (cooldownTimer >= cooldownPeriod)
        {
            switchState(state.dormant);
            cooldownTimer = 0;
        }
    }
    public void detectPlayer()
    {
        //probably play animation
        //change state
        //Debug.Log("player detected");

        switchState(state.chasing);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Health.instance.takeDamage(1);
        }
    }

    public void levelEnd()
    {
        //would say switch to dormant but may be weird if it just sits dormant
    }
}
