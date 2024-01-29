using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    public enum movementType
    {
        bounce,//bounces off walls
        hover,//hovers and goes point to point - spits as well
        teleport//teleports
    }
    [SerializeField]
    [Tooltip("The type of movement this enemy uses")]
    private movementType movementMode;
    
    private Rigidbody2D rb;

    Vector3 movementDir;
    [SerializeField]
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                //
                break;
            default:
                break;
        }
    }
}
