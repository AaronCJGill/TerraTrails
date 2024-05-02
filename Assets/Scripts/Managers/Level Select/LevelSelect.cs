using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //individual level
    //has ability to activate next level if completed
    [SerializeField]
    bool allowUnlock;
    [SerializeField]
    bool levelLocked = true;
    public bool LevelLocked
    {
        get{ return levelLocked; }
    }
    [SerializeField]
    bool levelComplete = false;
    bool levelMastered = false;
    [SerializeField]
    bool levelPlayed = false;//check if this level was played before
                             //if it wasnt then display min time to be "???" or dont display it at all
    [SerializeField]
    bool levelBought = false;
    [SerializeField]
    string levelName;
    public string LevelName
    {
        get { return levelName; }
    }
    [SerializeField]
    private float levelCost = 1;

    [SerializeField]
    LevelSelect _nextLevel;
    public LevelSelect previousLevel;
    [SerializeField]
    Transform topPos, rightPos, bottomPos, leftPos;
    Transform TopPos
    {
        get { return topPos; }
    }
    Transform RightPos
    {
        get { return rightPos; }
    }
    Transform BottomPos
    {
        get { return bottomPos; }
    }
    Transform LeftPos
    {
        get { return leftPos; }
    }
    public Transform ConnectedPoint
    {
        get; private set;

    }

    [SerializeField]
    GameObject cantBuylockedPanel;
    [SerializeField]
    GameObject canBuyLockPanel;
    [SerializeField]
    bool isAlwaysUnlocked;
    public bool IsAlwaysUnlocked
    {
        get { return isAlwaysUnlocked; }
    }
    public LevelSelect NextLevel
    {
        get { return _nextLevel; }
    }
    public Transform T
    {
        get { return transform; }
    }

    levelStats _levelstats;
    public levelStats LevelStats
    {
        get { return _levelstats; }
    }
    [SerializeField]
    private GameObject popUpUIPrefab;
    private mapLevelUIPopup instantiatedPopUp;

    [Header("New Sprites")]
    [SerializeField]
    Image sr;
    [SerializeField]
    private Sprite levelLockedSprite, levelBuyableSprite, levelUnlockedSprite;
    [Header("Pretty Name")]
    [SerializeField]
    private string UIDisplayName;
    private void Awake()
    {
        sr = GetComponent<Image>();
        if (string.IsNullOrEmpty(UIDisplayName))
        {
            UIDisplayName = levelName;
        }
    }
    private void Start()
    {
        checkStartingConditions(gameObject);

        if (isAlwaysUnlocked && LevelStatsManager.checkIfSaveFileExists(levelName))
        {
            //always unlock the second level if this level has been completed
            _levelstats = LevelStatsManager.instance.load(levelName);
            if (_levelstats.levelFinished())
            {
                if(_nextLevel != null)
                    NextLevel.unlockLevel();
            }

        }
        if (_nextLevel != null)
        {
            _nextLevel.previousLevel = this;
        }
        connectToNextLevelPoint();
    }

    public void checkStartingConditions( GameObject f)
    {
        //Debug.Log("Starting options + " + transform.name);
        //when starting, check if save exists, if it exists, load the stats here
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.LogError("Level Name Is Empty - Please Fix");
        }
        else if (isAlwaysUnlocked)
        {
            //check if we need to show stats (only show stats if the level has been played)
            if (LevelStatsManager.checkIfSaveFileExists(levelName))
            {
                levelPlayed = true;
            }
            else
            {
                levelPlayed = false;
            }
            levelLocked = false;
            levelBought = true;

            sr.sprite = levelUnlockedSprite;

            //cantBuylockedPanel.SetActive(false);
            //canBuyLockPanel.SetActive(false);
        }
        else
        {
            //if it exists, then load it up & it can only exist if it has been unlocked
            if (LevelStatsManager.checkIfSaveFileExists(levelName))
            {
                Debug.Log("Loading This Object");
                _levelstats = LevelStatsManager.instance.load(levelName);

                if (_levelstats.levelFinishedGold())
                {
                    //level completed and has been bought before
                    
                    levelMastered = true;
                    levelComplete = true;
                }
                else if (_levelstats.levelFinished())
                {
                    levelMastered = false;
                    levelComplete = true;
                }

                levelPlayed = true;
                levelLocked = false;
                levelBought = true;
                //cantBuylockedPanel.SetActive(false);
                //canBuyLockPanel.SetActive(false);

                sr.sprite = levelUnlockedSprite;
            }
            else
            {

                //level not played and file does not exist
                //this is called after the unlockLevel is called. Base everything off of what Unlock Level does
                //Debug.Log("Not found - " + levelName);
                if (allowUnlock)
                {
                    sr.sprite = levelBuyableSprite;
                    //cantBuylockedPanel.SetActive(false);
                    //canBuyLockPanel.SetActive(true);
                }
                else
                {
                    //cantBuylockedPanel.SetActive(true);
                    //canBuyLockPanel.SetActive(false);
                    sr.sprite = levelLockedSprite;
                }
                levelPlayed = false;
                //allowUnlock = false;
                levelMastered = false;
                levelComplete = false;
                levelBought = false;
                //Debug.Log("CANT BUY ------- " + transform.name);

            }
        }
    }
    public void lockLevel()
    {
        if (!isAlwaysUnlocked)
        {
            sr.sprite = levelLockedSprite;
            //cantBuylockedPanel.SetActive(true);
            //canBuyLockPanel.SetActive(false);
            allowUnlock = false;
            levelPlayed = false;
            //allowUnlock = false;
            levelMastered = false;
            levelComplete = false;
            levelBought = false;
        }

    }

    //connect to the net level's point
    void connectToNextLevelPoint()
    {
        Transform[] x = connectedPos();
        //if(x != null)
        //    Debug.Log(transform.name + " " + x[0].name + " -- " + x[1].parent.name + " "+ x[1].name);
    }

    /// <summary>
    /// first transform is the self point, second is the next point
    /// </summary>
    /// <returns></returns>
    Transform[] connectedPos()
    {

        //check if next point exists
        if (_nextLevel != null)
        {
            Transform selfPoint, nextPoint;
            Transform[] allSelfPoints = { topPos, rightPos, bottomPos, leftPos };
            selfPoint = allSelfPoints[0];

            Transform[] allNextPoints = { NextLevel.TopPos, NextLevel.RightPos, NextLevel.BottomPos, NextLevel.LeftPos };

            nextPoint = allNextPoints[0];

            float savedDistance = Vector2.Distance(selfPoint.position, nextPoint.position);

            for (int i = 0; i < allSelfPoints.Length; i++)
            {
                Transform TempSelfPoint = allSelfPoints[i];
                //Debug.Log(TempSelfPoint.name);
                for (int j = 0; j < allNextPoints.Length; j++)
                {
                    //Debug.Log(allNextPoints[i].name);
                    Transform TempNextPoint = allNextPoints[i];
                    //if this is the closest point pairing, then make these the saved values
                    if (Vector2.Distance(TempNextPoint.position, TempSelfPoint.position) < savedDistance)
                    {
                        //make this the closest point and save these two plus the distance
                        savedDistance = Vector2.Distance(TempNextPoint.position, TempSelfPoint.position);
                        nextPoint = TempNextPoint;
                        selfPoint = TempSelfPoint;
                        //Debug.Log(savedDistance + " " + nextPoint + nextPoint.transform.name + " " + selfPoint + transform.name);

                    }
                    //otherwise ignore
                    //Debug.Log(Vector2.Distance(TempNextPoint.position, TempSelfPoint.position) + " " + savedDistance);
                    
                }
            }

            Transform[] points = { selfPoint, nextPoint };

            return points;
        }

        return null;

    }


    [SerializeField]
    Transform popupspawnpoint;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Entered");
        //create popup UI
        GameObject popui;
        if (popupspawnpoint != null)
        {
            popui = Instantiate(popUpUIPrefab, popupspawnpoint.position, Quaternion.identity, transform.parent);
        }
        else
        {
            popui = Instantiate(popUpUIPrefab, topPos.position + Vector3.up * 2.25f, Quaternion.identity, transform.parent);
        }

        //Debug.Log(" best" + _levelstats.maxTimeCounter + " min " + _levelstats.minTime);
        instantiatedPopUp = popui.GetComponent<mapLevelUIPopup>();
        if (levelMastered)
        {
            //Debug.Log(_levelstats.devTime);
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.mastered, _levelstats, UIDisplayName);
        }
        else if (levelComplete || levelPlayed)
        {
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.played, _levelstats, UIDisplayName);
        }
        else if (levelLocked && !levelPlayed)
        {
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.unplayed, _levelstats, UIDisplayName, levelCost);
        }
        else if (levelLocked)
        {
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.locked, _levelstats, UIDisplayName, levelCost);

        }
        else if (!levelLocked && ! levelBought)
        {
            //level is not locked but isnt bought,
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.locked, _levelstats, UIDisplayName, levelCost);
        }
        else if (!levelPlayed && levelBought && !levelLocked || isAlwaysUnlocked)
        {
            //case all else fails
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.unplayed, _levelstats, UIDisplayName);
        }
    }

    public void unlockLevel()
    {
        levelLocked = false;
        allowUnlock = true;
        sr.sprite = levelBuyableSprite;
        //canBuyLockPanel.SetActive(true);
        //cantBuylockedPanel.SetActive(false);

        //this is essentially a function that goes through the chain of connected levels
        //originally is only called in the first level
        //goes checks to see if this level has been played already,
        //if they have, then allow them to unlock the next level

        Debug.Log("level select" + transform.name);

        if (NextLevel != null)
        {
            if (LevelStatsManager.checkIfSaveFileExists(levelName))
            {
                //always unlock the second level if this level has been completed
                _levelstats = LevelStatsManager.instance.load(levelName);
                sr.sprite = levelBuyableSprite;

                //canBuyLockPanel.SetActive(false);
                //cantBuylockedPanel.SetActive(false);
                if (_levelstats.levelFinished())
                {
                    //NextLevel.Invoke("unlockLevel", 5);
                    NextLevel.unlockLevel();
                }
            }
            else
            {
                //if the file does not exist and is unlocked, then change the lock texture
                //this is specifically for when the player has not bought the level
                Debug.Log("file does not exist but can buy level");
                //canBuyLockPanel.SetActive(true);
                //cantBuylockedPanel.SetActive(false);

                sr.sprite = levelBuyableSprite;
            }
        }


    }
    public void allowUnlockLevel()
    {
        levelBought = false;
        levelLocked = false;
    }
    public void buyLevel()
    {
        if (LevelStatsManager.canBuyLevel(levelCost) && !levelLocked && allowUnlock)
        {
            LevelStatsManager.buyLevel(levelCost);
            levelBought = true;
            _levelstats = new levelStats(levelName);
            LevelStatsManager.Save(_levelstats);
            sr.sprite = levelUnlockedSprite;
            //canBuyLockPanel.SetActive(false);
            //cantBuylockedPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Cannot purchase level");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (levelBought || isAlwaysUnlocked)
        {
            //go immediately into the level
            SceneManager.LoadScene(levelName);
        }
        else
        {
            buyLevel();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //destroy popUp
        Destroy(instantiatedPopUp.gameObject);
        instantiatedPopUp = null;
    }
    /*
    public void checkIfCanUnlock()
    {
        if (isAlwaysUnlocked)
        {
            //check if we need to show stats (only show stats if the level has been played)
            if (LevelStatsManager.checkIfSaveFileExists(levelName))
            {
                levelPlayed = true;
            }
            else
            {
                levelPlayed = false;
            }
            levelLocked = false;
            levelBought = true;
            cantBuylockedPanel.SetActive(false);
            canBuyLockPanel.SetActive(false);
        }
        else
        {
            //if it exists, then load it up & it can only exist if it has been unlocked
            if (LevelStatsManager.checkIfSaveFileExists(levelName))
            {
                Debug.Log("Loading This Object");
                _levelstats = LevelStatsManager.instance.load(levelName);

                if (_levelstats.levelFinishedGold())
                {
                    //level completed and has been bought before
                    levelMastered = true;
                    levelComplete = true;
                }
                else if (_levelstats.levelFinished())
                {
                    levelMastered = false;
                    levelComplete = true;
                }

                levelPlayed = true;
                levelLocked = false;
                levelBought = true;
                cantBuylockedPanel.SetActive(false);
                canBuyLockPanel.SetActive(false);
            }
            else
            {
                //level not played and file does not exist
                Debug.Log("Not found - " + levelName + " -------");
                levelPlayed = false;
                levelMastered = false;
                levelComplete = false;
                levelBought = false;

                cantBuylockedPanel.SetActive(true);
                canBuyLockPanel.SetActive(false);
            }
        }

        if (isAlwaysUnlocked && LevelStatsManager.checkIfSaveFileExists(levelName))
        {
            //always unlock the second level if this level has been completed
            _levelstats = LevelStatsManager.instance.load(levelName);
            if (_levelstats.levelFinished())
            {
                NextLevel.unlockLevel();
            }
        }
    
    }
    */

    //current problem, buying the level unlocks the level on the map screen
    //but doesnt affect the save since when the level is bought, the save does not exist
    //so if the player goes into a previous level, the will lose the fact that this level was not bought.

    //two solutions

    //if the player clicks, then the level has been bought. GO WITH THIS
    //if the level has been bought then go straight into the world.
    //this solves the problem of the player losing the fact that the level has been bought if they go to another level
    //if the player clicks, then the level has been bought. 
    //if the level is bought then create a save with the current levelSelect info
    //then we save over the level
}
