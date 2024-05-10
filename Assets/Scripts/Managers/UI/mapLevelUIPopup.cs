using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class mapLevelUIPopup : MonoBehaviour
{
    public enum levelInfoKnown
    {
        locked,
        unplayed,
        played,
        mastered
    }
    [SerializeField]
    GameObject unplayedPanel, playedPanel, masteredPanel, costPanel;
    [SerializeField]
    TextMeshProUGUI playedPanelGoalText, playedPanelPlayerText, masteredPanelPlayerText, masteredPanelGoalText, costPanelText;
    [SerializeField]
    TextMeshProUGUI levelNameText;
    [SerializeField]
    TextMeshProUGUI unplayedPanelPlayerTime, unplayedPanelGoalTime;
    public void init(levelInfoKnown lik, levelStats ls, string levelName, float cost = 0)
    {
        Debug.Log(ls.devTime +  " " + ls.minTime);
        switch (lik)
        {
            case levelInfoKnown.locked:
                //Debug.Log("Level locked");
                costPanel.SetActive(true);
                costPanelText.text = "Cost: " + cost;
                break;
            case levelInfoKnown.unplayed:
                unplayedPanel.SetActive(true);
                //Debug.Log("Level unPlayed");
                break;
            case levelInfoKnown.played:
                //Debug.Log("Level Played " + ls.maxTimeCounter);
                playedPanel.SetActive(true);
                playedPanelPlayerText.text = "Best Time: " + ls.maxTimeCounter;
                playedPanelGoalText.text = "Min Time: " + ls.minTime + "\nGold Time: " + ls.devTime; 
                break;
            case levelInfoKnown.mastered://TODO: Try to change the color of the text here
                //Debug.Log("Level Mastered " + ls.minTime + " " + ls.devTime);
                masteredPanel.SetActive(true);
                masteredPanelGoalText.text = "Best Time: " + ls.maxTimeCounter;
                masteredPanelPlayerText.text = "Min Time: " + ls.minTime + "\nGold Time: " + ls.devTime;
                break;
            default:
                break;
        }

        //set the name of the level
        if (!string.IsNullOrEmpty(levelName))
            levelNameText.text = levelName;
        else
            levelNameText.text = "???";//made it so that the level's name is not shown
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
