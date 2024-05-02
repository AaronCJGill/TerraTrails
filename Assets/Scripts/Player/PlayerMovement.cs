using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;
    [SerializeField][Tooltip("What percentage can players move the analog stick and not be at full speed ")][Range(0.1f,1f)]
    private float controllerThreshold = 0.8f;
    private Vector3 move;
    private Rigidbody2D rb;
    public bool canMove = true;
    public static PlayerMovement instance;
    GameObject spriteObject;
    private SpriteRenderer sr;
    private Animator animator;
    float dashMultiplier = 1;
    public Vector2 PositionPlusMove(float movemult)
    {
       
        //Debug.Log("current pos: "+ transform.position + " || Predicted pos: " + (transform.position + ((move * movementSpeed * dashMultiplier * Time.fixedDeltaTime) * movemult)) + " || Move mult: " + movemult);
        return transform.position + ((move * movementSpeed * dashMultiplier * Time.fixedDeltaTime) * movemult);
    }
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = transform.GetChild(0).GetComponent<Animator>();
        sr = animator.gameObject.GetComponent<SpriteRenderer>();
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }

        //powerup is activated and ready as soon as player gets into the scene
        instance.animator.ResetTrigger("hasDied");
        AbilityManager.activatePowerup();
    }

    
    private void Start()
    {
        spriteObject = transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if (canMove)
        {

            move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

            //makes it so that controller movement is more viable, yet keyboard movement is near instant still
            
            if (move.x > controllerThreshold)
                move.x = 1;
            if (move.x < -controllerThreshold)
                move.x = -1;
            //makes it so that controller movement is more viable, yet keyboard movement is near instant still
            if (move.y > controllerThreshold)
                move.y = 1;
            if (move.y < -controllerThreshold)
                move.y = -1;
            //if(move != Vector3.zero)
                //Debug.Log(move);

            //move left and right at a consistent rate
            /*
            if (move.x > 0)
                move.x = 1;
            if (move.x < 0)
                move.x = -1;
            
            //move up and down at a consistent rate

            if (move.y > 0)
                move.y = 1;
            if (move.y < 0)
                move.y = -1;
            */
            //stop the player from moving faster than they should when moving diagonally
            //move = move.normalized;
            //Debug.Log(move);
            //move.Normalize();


            if (Input.GetKeyDown(KeyCode.R))
            {
                //Health.instance.takeDamage(1);
            }
        }
        else
        {
            move = Vector2.zero;
        }


        //animation details
        //if player is moving in any direction, 
        float s = 0;
        if (move.x != 0 || move.y != 0)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                s = Input.GetAxisRaw("Horizontal");
            else
                s = Input.GetAxisRaw("Vertical");

            s = Mathf.Abs(s);

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                sr.flipX = true;
            }
            else if(Input.GetAxisRaw("Horizontal") < 0)
            {
                sr.flipX = false;
            }
        }
        //print(s);
        animator.SetFloat("Speed", s);
        //Debug.Log(animator.GetFloat("Speed"));
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + move * movementSpeed * dashMultiplier * Time.fixedDeltaTime);
    }
    public static void kill()
    {
        instance.canMove = false;


        //fix this later
        //instance.spriteObject.SetActive(false);
        //Debug.Log("kill");
        instance.animator.SetTrigger("hasDied");
    }

    public void doKillAnim()
    {
        //Debug.Log("kill");
        instance.animator.SetTrigger("hasDied");
    }
    public void playerDash(float pdM = 1)
    {
        dashMultiplier = pdM;
    }

}
