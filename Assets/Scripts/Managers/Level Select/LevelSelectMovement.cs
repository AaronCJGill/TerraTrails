using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMovement : MonoBehaviour
{
    //holds information on where player currently is
    //moves from the current position to whatever direction the player selects,
    //only if there is a level select node there already
    //moves similar to a mario level map
    [SerializeField][Tooltip("This is the level the player starts at, with each level acting")]
    LevelSelect startingLevel;
    LevelSelect currentLevel;
    bool inTransit = false;
    float moveTimeMax = 0.25f;
    float timer = 0;
    private void Start()
    {
        //go to the last unlocked level
        findLastAvailableLevel();
    }

    private void findLastAvailableLevel()
    {
        LevelSelect cls = startingLevel;
        //loops through all of the levels and logs which level is the last available one
        // each level has a reference to the next, and has information available on whether or not it is loaded
        bool lastLevelFound = false;
        while (!lastLevelFound)
        {
            //if the next level is always unlocked, or if the next level's save exists, then this level is the next suitable target
            LevelSelect nextLev = cls.NextLevel;
            if (cls.NextLevel.IsAlwaysUnlocked || LevelStatsManager.checkIfSaveFileExists(cls.NextLevel.LevelName))
            {
                //always unlock the second level if this level has been completed
                cls = nextLev;
            }
            else
            {
                lastLevelFound = true;
            }
        }

        //spawn in at whatever the next level was found at
        transform.position = cls.T.position;

        currentLevel = cls;
    }


    private void move()
    {
        if (!inTransit)
        {
            //need way of translating what direction player chooses to in game pathed direction
            if (Input.GetAxis("Horizontal") > 0.5f && currentLevel.NextLevel && !currentLevel.NextLevel.LevelLocked)
                StartCoroutine(moveToNextPoint(currentLevel.NextLevel));
            //Debug.Log(Input.GetAxis("Horizontal"));
            else if (Input.GetAxis("Horizontal") < -0.5f && currentLevel.previousLevel && !currentLevel.previousLevel.LevelLocked)
                StartCoroutine(moveToNextPoint(currentLevel.previousLevel));
        }
    }

    private void Update()
    {
        move();
    }

    private void levelInteract()
    {
        //if this level is able to be unlocked, then attempt to unlock it

        //if this level is able to be played, then play it

        if (currentLevel.LevelLocked)
        {

        }
    }


    IEnumerator moveToNextPoint(LevelSelect nextLevel)
    {
        inTransit = true;
        Vector2 startPos = transform.position;
        Vector2 endPos = nextLevel.transform.position;
        while (timer < moveTimeMax)
        {
            transform.position = Vector2.Lerp( startPos, endPos, timer / moveTimeMax);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        transform.position = endPos;
        currentLevel = nextLevel;
        inTransit = false;
    }


}
