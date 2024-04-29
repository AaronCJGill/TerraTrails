using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementTeleport : MonoBehaviour, enemyDestroy
{
    [Header("Behavior Time Settings")]
    bool doesSpawnRoutine = true;
    [SerializeField]
    float waitTime = 4f;
    [SerializeField][Tooltip("Time the disappear animation needs to be complete")]
    float disappearAnimTime = 2;
    float disTimer = 0;
    [SerializeField]
    SpriteRenderer sr;

    [SerializeField]
    [Tooltip("Time this takes before waiting")]
    float moveWaitTime = 2f;
    [SerializeField]
    [Tooltip("Time this takes to wait before spitting")]
    float spitWaitTime = 2f;
    [SerializeField]
    [Tooltip("Amount of time this enemy disappears for")]
    float totalDisappearTime = 2f;
    [SerializeField]
    [Tooltip("Amount of time this enemy prepares to teleport for before disappearing")]
    float teleportPrepTime = 1.1f;
    [SerializeField]
    [Tooltip("Amount of time this enemy takes before teleporting")]
    float teleportWaitTIme = 0.5f;
    [SerializeField]
    [Tooltip("Amount of time this takes to restart move and attack behavior")]
    float restartTime = 1f;
    [Header("Warning Symbol")]
    [SerializeField]
    GameObject warningSymbol;

    [Header("Behavior Percentage")]
    [SerializeField]
    [Tooltip("Chance for this to spawn onto the player position")]
    [Range(0, 100)]
    int TeleToPlayer = 25;

    [SerializeField]
    Animator anim;

    BoxCollider2D cd;


    //wait, teleport to random spot or on top of player, repeat

    [Header("Pathfinding Settings")]
    [SerializeField]
    private Transform storedTransform;
    [SerializeField]
    Pathfinding.Seeker seeker;
    [SerializeField]
    Pathfinding.AIPath pathing;
    [SerializeField]
    Pathfinding.AIDestinationSetter destinationSetter;

    [SerializeField]
    [Tooltip("Keep this as 10 if you want it to stay the default value.")]
    float walkAcceleration = 1000;
    [SerializeField]
    float stopDistance = 0.3f;
    [SerializeField]
    float picknextwaypointdistance = 0.8f;
    [SerializeField][Tooltip("Doesnt scale to units or to seconds. Dont ask me what the fuck it does. Set it somewhere between 10 and 50 and it will work")]

    float predictionDistance = 10f;
    IEnumerator Movement()
    {
        disTimer = 0;
        //spawn in a warning symbol as soon as this is ready to teleport
        GameObject GS = Instantiate(warningSymbol, transform.position, Quaternion.identity);//create a warning symbol for the area
        GS.GetComponent<WarningSymbolBehavior>().init(teleportPrepTime - 0.1f);


        


        yield return new WaitForSeconds(teleportPrepTime);


        //animation of it disappearing
        anim.SetTrigger("digDown");

        int rnum = Random.Range(0, 101);
        Vector2 spawnPos;
        if (rnum >= TeleToPlayer)//if it is below ~25, teleport to player position
            spawnPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        else
            spawnPos = PlayerMovement.instance.transform.position;

        yield return new WaitForSeconds(2.0f);
        //do appearing animation and random walking animation
        

        //spawn in explosion or warning symbol
        
        GameObject WS = Instantiate(warningSymbol, spawnPos, Quaternion.identity);//create a warning symbol for the area
        WS.GetComponent<WarningSymbolBehavior>().init(moveWaitTime, true);
        yield return new WaitForSeconds(moveWaitTime);
        transform.position = spawnPos;
        disTimer = 0;
        //emerge animation
        anim.ResetTrigger("digDown");
        anim.SetTrigger("digUp");


        yield return new WaitForSeconds(restartTime);
        StartCoroutine(Movement());

    }



    //dig down, move offscreen when anim is done,
    //dig up at new spot, wander around for a while
    float walkSpeed = 3;
    float walkTimeTotal = 4f;
    float walktimer = 0;
    [SerializeField]
    float randomPointDestinationDistance = 4;
    Vector2 randomDestinationPoint;
    
    IEnumerator newteleportbehavior()
    {
        //dig down
        anim.SetTrigger("digDown");
       // Debug.Log("starting dig down");
        yield return new WaitForSeconds(1.7f);
        //move this character off screen 
        transform.position = new Vector3(100, 100);
        //Debug.Log("starting dig down");


        //disappear for a set amount of time
        yield return new WaitForSeconds(totalDisappearTime);

        //dig up
        //Debug.Log("digging back up");
        anim.ResetTrigger("digDown");
        anim.SetTrigger("digUp");
        //appear in new position
        int rnum = Random.Range(0, 101);
        Vector2 spawnPos;
        bool playerlocvalid = false;
        Vector2 playerLoc = PlayerMovement.instance.PositionPlusMove(predictionDistance);//multiplier to predict where the player will be
        int spawnloctries = 0;
        //check if player location is outside of bounds
        while (playerlocvalid  == false|| spawnloctries < 100)
        {
            //if within the bounds of the levelboundaries
            if (playerLoc.x < LevelBoundary.BottomRightBound.X &&
                playerLoc.x > LevelBoundary.leftTopBound.X &&
                playerLoc.y > LevelBoundary.BottomRightBound.Y &&
                playerLoc.y < LevelBoundary.leftTopBound.Y
                )
            {
                //check if predicted location is inside wall
                Collider[] hits = Physics.OverlapSphere(playerLoc, 1, 6, QueryTriggerInteraction.Ignore);
                //Debug.Log("Finding point inside ");
                if (hits.Length == 0)
                {
                    //Debug.Log("Good player position found");
                    //get out of loop when a free point is found
                    playerlocvalid = true;
                    spawnloctries = 99999;
                    break;
                }
            }
            else
            {
                playerLoc = PlayerMovement.instance.PositionPlusMove(predictionDistance - (float) (spawnloctries / 2));
                Debug.Log("cant find good position found");
            }
            //stay in this loop until its exhausted

            yield return null;
        }

        if (rnum >= TeleToPlayer)//if it is below ~25, teleport to player position
            spawnPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        else if (!playerlocvalid)//still need to spawn on player but cant find good position
            spawnPos = PlayerMovement.instance.Position;
        else
            spawnPos = playerLoc;//spawn at where ever the player is currently

        transform.position = spawnPos;
        //set collider to off
        cd.enabled = false;
        //wait for duration of animation
        //Debug.Log("Beforecanmove");
        yield return new WaitForSeconds(2.3f);
        //Debug.Log("AfterCanlos");

        //should automatically go to the walking animation so we dont have specific trigger for walking
        cd.enabled = true;
        //set collider back on
        pathing.canMove = true;


        //randomDestinationPoint = usefulFunctions.positioning.getRandomSpawnPoint();
        storedTransform.position = usefulFunctions.positioning.PickRandomPointNearby(transform.position, 3);//somewhere within 2 units

        walktimer = 0;
        //do random walking, going to multiple places
        while (walktimer < walkTimeTotal)
        {
            if (Vector2.Distance(transform.position, storedTransform.position) > 0.5f)
            {
                //Debug.Log("MovingTowards");
                if (storedTransform.position.x > transform.position.x)
                {
                    //random position is to the right
                    sr.flipX = true;
                }
                else
                {
                    //random position is to the left
                    sr.flipX = false;
                }
                //if we are faw away from distance point, just move
                //transform.position = Vector2.MoveTowards(transform.position, randomDestinationPoint, walkSpeed * Time.deltaTime);
                
                walktimer += Time.deltaTime;
                yield return null;
            }
            else //we are close to the point and should choose next point
            {
                //Debug.Log("Finding random Spot");
                //Debug.Log("getting random point");
                storedTransform.position = usefulFunctions.positioning.PickRandomPointNearby(transform.position, randomPointDestinationDistance);//somewhere within 2 units
                //make sure the point is close enough
            }
        }
        
        pathing.canMove = false;

        //restart the animation
        anim.ResetTrigger("digUp");
        //restart system
        playerlocvalid = true;
        spawnloctries = 99999;
        StartCoroutine(newteleportbehavior());
    }



    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(Movement());
    }

    private void Update()
    {
        //pathing.canMove = true;
    }
    private void Start()
    {
       
        //sr = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Pathfinding.Seeker>();
        pathing = GetComponent<Pathfinding.AIPath>();
        destinationSetter = GetComponent<Pathfinding.AIDestinationSetter>();
        pathing.maxSpeed = walkSpeed;
        pathing.slowdownDistance = stopDistance;
        pathing.pickNextWaypointDist = picknextwaypointdistance;
        //pathing.maxa
        storedTransform = new GameObject("Teleporter " + "Target -- " + name).transform;
        //pathing.canMove = false;
        pathing.canMove = true;
        storedTransform.position = transform.position;
        destinationSetter.target = storedTransform;
        pathing.maxAcceleration = walkAcceleration;
        
        cd = GetComponent<BoxCollider2D>();
        //spawn in phase
        if (doesSpawnRoutine)
        {
            //play animation and stand still
            spawnSequence();
        }
        else
        {
            StartCoroutine(newteleportbehavior());
        }
    }
    void spawnSequence()
    {
        //do something here
        //play animation and stand still

        StartCoroutine(newteleportbehavior());
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


/*Color startColor = sr.color;
        
        while (disTimer < disappearAnimTime)
        {
            //currentOpacity = Mathf.Lerp(startOpacity, finalOpacity, time / destroyTime);
            disTimer += Time.deltaTime;
            //sr.color = Color.Lerp(startColor, finalColor, time / destroyTime);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, EasingFunction.EaseInCubic(startColor.a, 0, disTimer / disappearAnimTime));
            //sr.color = new Color(startColor.r, startColor.g, startColor.b, currentOpacity);
            //Debug.Log(currentOpacity);
            yield return null;

        }
        
        //when animation is over, change it back to its regular colors, and move it out of the way
        //disappears for a second
        sr.color = startColor;
        transform.position = new Vector2(30,30);
        */

/*
        //do fade in animation
        Color fadeInStart = new Color(startColor.r, startColor.g, startColor.b, 0);
        sr.color = fadeInStart;
        while (disTimer < disappearAnimTime)
        {


            //currentOpacity = Mathf.Lerp(startOpacity, finalOpacity, time / destroyTime);
            disTimer += Time.deltaTime;
            //sr.color = Color.Lerp(startColor, finalColor, time / destroyTime);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, EasingFunction.EaseInCubic(0, startColor.a, disTimer / disappearAnimTime));
            //sr.color = new Color(startColor.r, startColor.g, startColor.b, currentOpacity);
            //Debug.Log(currentOpacity);
            yield return null;

        }

        sr.color = startColor;
        */