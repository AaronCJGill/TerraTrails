using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;
    private Vector3 move;
    private Rigidbody2D rb;
    public bool canMove = true;
    public static PlayerMovement instance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }

    }

    void Update()
    {
        if (canMove)
        {

            move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

            //move left and right at a consistent rate
            if (move.x > 0)
                move.x = 1;
            if (move.x < 0)
                move.x = -1;

            //move up and down at a consistent rate
            if (move.y > 0)
                move.y = 1;
            if (move.y < 0)
                move.y = -1;

            //stop the player from moving faster than they should when moving diagonally
            //move = move.normalized;
            //Debug.Log(move);
            move.Normalize();
            //Debug.Log(move);


            if (Input.GetKeyDown(KeyCode.R))
            {
                Health.instance.takeDamage(1);
            }
        }
        else
        {
            move = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + move * movementSpeed * Time.fixedDeltaTime);
    }
    public static void kill()
    {
        instance.canMove = false;

    }

}
