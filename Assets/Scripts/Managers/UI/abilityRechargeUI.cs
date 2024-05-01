using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abilityRechargeUI : MonoBehaviour
{
    public static abilityRechargeUI instance;
    //recharge time, recharge type
    float rechargetimetotal;
    float currentrechargetime = 0;

    float barHeightMax = 144;
    float barHeightStarting = 0;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("Something has gone extremely wrong with ability recharge ui");
            Destroy(gameObject);
        }
        else if(instance == null)
        {
            instance = this;
        }
    }
    public enum abilitytype
    {
        parry, dash
    }
    private abilitytype at;
    //refernce to recharge bar 
    public void initialize(abilitytype type, float rechargetimetotal)
    {
        at = type;
    }
    public void abilityUsed()
    {
        
    }
    private void handleRecharge()
    {

    }
    private void Update()
    {
        handleRecharge();
    }
}
