using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBouncing : MonoBehaviour, enemyDestroy
{
    [SerializeField]
    private float speed = 10;
    public bool doesSpawnRoutine;

    float xdir, ydir;
    float xvel, yvel;
    Vector3 vel;
    Rigidbody2D rb;

    Vector3 lastPos;
    float smallestDist = 100;
    private void Start()
    {
        if (doesSpawnRoutine)
        {
            //animation
            spawnSequence();
        }
        rb = GetComponent<Rigidbody2D>();

        //randomize x and y directions

        int lorr = Random.Range(0, 2);
        if (lorr == 0)
        {
            xdir = -1;
        }
        else
        {
            xdir = 1;
        }

        int uord = Random.Range(0, 2);
        if (uord == 0)
        {
            ydir = -1;
        }
        else
        {
            ydir = 1;
        }


        xvel = xdir * speed;
        yvel = ydir * speed;
    }

    void spawnSequence()
    {
        //animation
    }

    // Update is called once per frame
    void Update()
    {

        xvel = xdir * speed;
        yvel = ydir * speed;

        vel = new Vector3(xvel, yvel) ;

        if (Input.GetKey(KeyCode.K))
            smallestDist = 100;
    }
    private void FixedUpdate()
    {
        //rb.velocity = vel;
        lastPos = transform.position;
        rb.MovePosition(transform.position + vel * Time.fixedDeltaTime);
        //Debug.Log   ((transform.position + vel * Time.fixedDeltaTime) + " - " + lastPos + " - "+ Vector2.Distance(lastPos, transform.position));

        if (Vector2.Distance(lastPos, transform.position) < smallestDist)
        {
            smallestDist = Vector2.Distance(lastPos, transform.position);
        }
        //if position.x has not changed but still going to same location

        Debug.Log(smallestDist);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Health.instance.takeDamage(1);
        }
        else
        {
            //simplified equation
            //if there is a collision, it has a normal.
            //This normal will be 1 or -1 in a given position 
            //we find the absolute value and see if it equals 1 and flip its direction
            //we can adjust this to see if the value is simple negative or positive, to have it bounce off slopes if needed
            if (Mathf.Abs(collision.contacts[0].normal.x) == 1)
            {
                //(1,0) is left collision
                //(-1,0) is right collision
                xdir *= -1;
            }
            //for some reason the y is strange, it will calculate .999999 instead of 1 internally but display 1 to us.
            //if (collision.contacts[0].normal.y >= 0.1 || collision.contacts[0].normal.y <= -0.1)

            if (Mathf.Abs(collision.contacts[0].normal.y) >= 0.1)
            {
                ydir *= -1;
                //ydir *= -1;
            }
            //Debug.Log(ydir + " " + yvel + " " + (int) collision.contacts[0].normal.y);

        }

    }
    public void levelEnd()
    {
        StopAllCoroutines();
    }
}
