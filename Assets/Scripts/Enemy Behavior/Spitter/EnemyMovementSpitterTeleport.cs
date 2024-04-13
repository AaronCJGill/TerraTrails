using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementSpitterTeleport : MonoBehaviour, enemyDestroy
{
    [Header("Behavior Time Settings")]
    bool doesSpawnRoutine = true;
    [SerializeField]
    [Tooltip("Amount of time this enemy prepares to teleport for before disappearing")]
    float teleportPrepTime = 1.1f;

    [SerializeField]
    [Tooltip("Time this takes to disappear")]
    float disappearAnimTime = 2;
    [SerializeField]
    [Tooltip("Amount of time disappeared time varies by")]
    float teleportVaryTime = 0.2f;
    float disTimer = 0;
    [SerializeField]
    [Tooltip("Amount of time this enemy takes before ")]
    float teleportWaitTIme = 0.5f;
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
    [Tooltip("Amount of time this takes to restart move and attack behavior")]
    float restartTime = 1f;

    SpriteRenderer sr;
    [SerializeField]
    [Tooltip("Chance for this to spawn onto the player position")]
    [Range(0,100)]
    int TeleToPlayer = 25;
    [SerializeField]
    GameObject projectile;

    [Header("Warning Settings")]
    [SerializeField]
    GameObject warningSymbol;
    [SerializeField]
    bool doesTeleportWaitTIme = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Movement());
        sr = GetComponent<SpriteRenderer>();
        if (doesSpawnRoutine)
        {
            
        }
        else
        {
            StartCoroutine(Movement());
        }

    }

    void spawnRoutine()
    {
        //do something

        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        disTimer = 0;
        //spawn in a warning symbol as soon as this is ready to teleport
        GameObject GS = Instantiate(warningSymbol, transform.position, Quaternion.identity);//create a warning symbol for the area
        GS.GetComponent<WarningSymbolBehavior>().init(teleportPrepTime - 0.1f);
        yield return new WaitForSeconds(teleportPrepTime);

        //animation of it disappearing
        Color startColor = sr.color;
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
        transform.position = new Vector2(30, 30);


        int rnum = Random.Range(0,101);
        Vector2 spawnPos;
        if (rnum >= TeleToPlayer)//if it is below ~25, teleport to player position
            spawnPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        else
            spawnPos = PlayerMovement.instance.transform.position;

        float rvariation = Random.Range(-teleportVaryTime, teleportVaryTime);
        if (doesTeleportWaitTIme)
            yield return new WaitForSeconds(teleportWaitTIme + rvariation);


        //maybe new spawn delay so its gone for one second
        GameObject WS = Instantiate(warningSymbol, spawnPos, Quaternion.identity);//create a warning symbol for the area
        WS.GetComponent<WarningSymbolBehavior>().init(moveWaitTime, true);
        yield return new WaitForSeconds(moveWaitTime);
        transform.position = spawnPos;
        disTimer = 0;
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

        //wait a second, spit
        yield return new WaitForSeconds(spitWaitTime);
        Instantiate(projectile, transform.position, Quaternion.identity);


        yield return new WaitForSeconds(restartTime);
        StartCoroutine(Movement());

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
