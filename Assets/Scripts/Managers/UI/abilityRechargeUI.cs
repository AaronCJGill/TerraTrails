using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class abilityRechargeUI : MonoBehaviour
{
    //this attaches to the left bar on the game canvas in each scene
    //and should be hooked up accordingly
    public static abilityRechargeUI instance;
    float rechargetimetotal;
    float currentrechargetime = 0;

    float barHeightMax = 144;
    float barHeightStarting = 0;
    [SerializeField]
    RectTransform rechargebar;
    [SerializeField]
    TextMeshProUGUI abilityprompt;
    [SerializeField]
    GameObject dashSprite, parrySprite;
    Vector2 startv, endv;
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

            //assume that the player does not trigger this 
            abilityprompt.text = "Nothing";
            abilityUsed();
            endv = new Vector2(48, barHeightMax);
            startv = new Vector2(48, barHeightStarting);
        }
    }
    public enum abilitytype
    {
        parry, dash, none
    }
    private abilitytype at = abilitytype.none;
    //refernce to recharge bar 
    
    public void initialize(abilitytype type, float rechargetimetotal)
    {
        at = type;
        this.rechargetimetotal = rechargetimetotal;
        if (type == abilitytype.parry)
        {
            dashSprite.SetActive(false);
            parrySprite.SetActive(true);
            abilityprompt.text = "Parry";
        }
        else if(type == abilitytype.dash)
        {
            dashSprite.SetActive(false);
            parrySprite.SetActive(true);
            abilityprompt.text = "Dash";
        }
        else
        {
            parrySprite.SetActive(true);
            dashSprite.SetActive(false);
            abilityprompt.text = "Nothing";
        }
    }
    public void abilityUsed()
    {
        currentrechargetime = 0;
        rechargebar.sizeDelta = new Vector2(48, 0);
    }
    private void handleRecharge()
    {
        if (at != abilitytype.none)
        {
            currentrechargetime += Time.deltaTime;
            rechargebar.sizeDelta = Vector2.Lerp(startv, endv, currentrechargetime / rechargetimetotal);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            abilityUsed();
        }
        handleRecharge();
    }
}
