using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyBehavior : MonoBehaviour
{

    public action[] ActionList;
    private int currentPos = 0;

    //should have a list of things to do
    //0 - attack,
    //1 - move to random point
    //2 - wait
    //repeat
    [System.Serializable]
    public class action
    {
        public enum actionType
        {
            attack,
            movement,
            wait
        }
        public actionType act;


    }

    void cycleBehavior()
    {
        //when called, will choose to do item on item
        currentPos++;
        if (currentPos > ActionList.Length)
        {
            currentPos = 0;
        }



    }


}
