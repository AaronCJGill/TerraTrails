using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float waitTime = 2.5f;
    public bool doesSpawnRoutine;
    [SerializeField]
    [Tooltip("Chance for this to charge to the player position")]
    [Range(0, 100)]
    int chargeToPlayer = 75;
    [Header("Randomization Settings")]
    [SerializeField]
    float chargeRandomization = 0.5f;

    SpriteRenderer sr;
    [SerializeField]
    float startupWaitTime = 1f;

    bool playerIsNotDead;

    void Start()
    {
        if (doesSpawnRoutine)
        {
            //animation
            spawnSequence();
        }
        else
        {
            //StartCoroutine(move());
        }
    }

    void spawnSequence()
    {
        //StartCoroutine(movement(startupWaitTime));
    }

    void movement()
    {
        
    }

    private void Update()
    {
        playerIsNotDead = !Health.isDead;
    }

    void afterDeathWander()
    {

    }

}
