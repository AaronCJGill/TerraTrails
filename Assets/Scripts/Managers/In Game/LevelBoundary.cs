using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundary : MonoBehaviour
{
    public enum boundType
    {
        topleft,
        bottomright
    }
    public boundType bt;
    public static LevelBoundary leftTopBound;
    public static LevelBoundary BottomRightBound;
    public float X
    {
        get
        {
            return transform.position.x;
        }
    }
    public float Y
    {
        get
        {
            return transform.position.y;
        }
    }

    private void Awake()
    {
        if (bt == boundType.topleft)
        {
            if (leftTopBound != this && leftTopBound != null)
            {
                Destroy(gameObject);
            }
            else if (leftTopBound == null)
            {
                leftTopBound = this;
            }
        }
        else if (bt == boundType.bottomright)
        {
            if (BottomRightBound!= this && BottomRightBound!= null)
            {
                Destroy(gameObject);
            }
            else if (BottomRightBound == null)
            {
                BottomRightBound = this;
            }
        }
    }
}
