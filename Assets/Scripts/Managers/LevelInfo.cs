using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [Header("Information about the level")]
    [Tooltip("If this is true, then the level has a time limit. Set to false on menu.")]
    public bool isTimed = true;
    [Tooltip("Amount of time for this level to be active")]
    public float timerAmount = 15;

    private void Start()
    {
        if (isTimed)
        {
            GameManager.levelStart(timerAmount);
        }
        else
        {
            GameManager.levelStart();
        }
    }
}
