using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public static HealthUI instance;

    [SerializeField]
    GameObject[] healthUI = new GameObject[5];

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
    }

    public void updateHealthUI()
    {
        //Debug.Log(Health.instance.health);
        for (int i = 0; i < healthUI.Length; i++)
        {
            //if health is == healthui
            if (Health.instance.health > i)
            {
                healthUI[i].SetActive(true);
            }
            else
            {
                healthUI[i].SetActive(false);
            }
        }


    }

}
