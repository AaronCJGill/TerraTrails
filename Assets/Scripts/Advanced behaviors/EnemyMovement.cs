using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType
    {
        teleport,
        charging,
        hover,

    }
    public MovementType movement;

    public void movePosition()
    {

    }



    /*
    public enum movementType
    {
        bounce,//bounces off walls
        hover,//hovers and goes point to point - spits as well
        teleport//teleports
    }
    [SerializeField]
    [Tooltip("The type of movement this enemy uses")]
    private movementType movementMode;

    private EnemyAttackBehavior attackBehavior;
    
    //list of actions that this enemy will cycle through
    //an action series may be (move, attack, wait, repeat)

    private Rigidbody2D rb;
    private Vector2 movementDir;
    [SerializeField]
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackBehavior = GetComponent<EnemyAttackBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }


    private void movement()
    {
        switch (movementMode)
        {
            case movementType.bounce:
                //once it hits a wall, move the other direction from where it hit something
                
                break;
            case movementType.hover:
                //stands still
                //charges ranged attack
                //completes ranged attack
                //moves to random point on screen



                break;
            case movementType.teleport:
                //choose random point to teleport to
                //disappear for 1 second (display warning of area its going to move to)
                


                break;
            default:
                break;
        }
    }
    */
}
