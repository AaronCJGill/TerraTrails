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
    bool levelLocked = true;
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
    [SerializeField]
    private float levelCost = 1;

    [SerializeField]
    LevelSelect _nextLevel;
    [SerializeField]
    Transform topPos, rightPos, bottomPos, leftPos;
    [SerializeField]
    GameObject cantBuylockedPanel;
    [SerializeField]
    GameObject canBuyLockPanel;
    [SerializeField]
    bool isAlwaysUnlocked;
    public LevelSelect NextLevel
    {
        get { return _nextLevel; }
    }
    levelStats _levelstats;
    public levelStats LevelStats
    {
        get { return _levelstats; }
    }
    [SerializeField]
    private GameObject popUpUIPrefab;
    private mapLevelUIPopup instantiatedPopUp;

    private void Awake()
    {


        

    }
    private void Start()
    {
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
                //Debug.Log("Not found - " + levelName);
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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Entered");
        //create popup UI
        GameObject popui = Instantiate(popUpUIPrefab, topPos.position + Vector3.up * 2.25f, Quaternion.identity, transform.parent);
        //Debug.Log(" best" + _levelstats.maxTimeCounter + " min " + _levelstats.minTime);
        instantiatedPopUp = popui.GetComponent<mapLevelUIPopup>();
        if (levelMastered)
        {
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.mastered, _levelstats, levelName);
        }
        else if (levelComplete || levelPlayed)
        {
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.played, _levelstats, levelName);
        }
        else if (levelLocked && !levelPlayed)
        {
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.unplayed, _levelstats, levelName, levelCost);
        }
        else if (levelLocked)
        {
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.locked, _levelstats, levelName, levelCost);

        }
        else if (!levelLocked && ! levelBought)
        {
            //level is not locked but isnt bought,
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.locked, _levelstats, levelName, levelCost);
        }
        else if (!levelPlayed && levelBought && !levelLocked || isAlwaysUnlocked)
        {
            //case all else fails
            instantiatedPopUp.init(mapLevelUIPopup.levelInfoKnown.unplayed, _levelstats, levelName);
        }
    }

    public void unlockLevel()
    {
        levelLocked = false;
        canBuyLockPanel.SetActive(true);
        cantBuylockedPanel.SetActive(false);
        
        //this is essentially a function that goes through the chain of connected levels
        //originally is only called in the first level
        //goes checks to see if this level has been played already,
        //if they have, then allow them to unlock the next level

        if (NextLevel != null)
        {
            if (LevelStatsManager.checkIfSaveFileExists(levelName))
            {
                //always unlock the second level if this level has been completed
                _levelstats = LevelStatsManager.instance.load(levelName);
                canBuyLockPanel.SetActive(false);
                cantBuylockedPanel.SetActive(false);
                if (_levelstats.levelFinished())
                {
                    NextLevel.unlockLevel();
                }
            }
            else
            {
                //if the file does not exist and is unlocked, then change the lock texture
                //this is specifically for when the player has not bought the level
                canBuyLockPanel.SetActive(true);
                cantBuylockedPanel.SetActive(false);
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
        if (LevelStatsManager.canBuyLevel(levelCost) && !levelLocked)
        {
            levelBought = true;
            _levelstats = new levelStats(levelName);
            canBuyLockPanel.SetActive(false);
            cantBuylockedPanel.SetActive(false);
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
                //Debug.Log("Not found - " + levelName);
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
