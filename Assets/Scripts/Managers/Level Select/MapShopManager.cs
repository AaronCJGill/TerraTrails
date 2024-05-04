using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MapShopManager : MonoBehaviour
{
    //for right now, there should be a way to move the main level objects a certain direction off screen, 
    // just because idk andrea's plans for how the main menu should look
    //ideally the player would move to the shop on the map, but idk how it looks on andrea's side of things
    [SerializeField]
    GameObject parryButton, dashButton;
    //[SerializeField]
    //TextMeshProUGUI parryButtonText, dashButtonText;
    [SerializeField]
    float parryCost = 50;
    [SerializeField]
    float dashCost = 50;
    Abilities pA;
    [SerializeField]
    TextMeshProUGUI parryboughtText, dashboughttext, equippedtext;
    public static MapShopManager instance;
    [SerializeField]
    private GameObject parrytutorialobject, dashtutorialobject;
    [SerializeField]
    private TextMeshProUGUI currencyText;
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        pA = AbilityManager.instance.savedAbilities;
        parrytutorialobject.SetActive(false);
        dashtutorialobject.SetActive(false);
    }
    public void shopActivated()
    {
        updateText();
    }
    public void updateText()
    {
        switch (pA.equippedAbility)
        {
            case Abilities.AbilityType.nothing:
                equippedtext.text = "Nothing is equipped";
                break;
            case Abilities.AbilityType.dash:
                equippedtext.text = "Dash is equipped";
                break;
            case Abilities.AbilityType.parry:
                equippedtext.text = "Parry is equipped";
                break;
            default:
                break;
        }

        if (pA.dashUnlocked)
        {
            dashboughttext.text = "Dash is unlocked";
            //dashButtonText.text = "Equip Dash";
        }
        else
        {
            dashboughttext.text = "Dash is not unlocked : " + dashCost;
            //dashButtonText.text = "Buy Dash";
        }
        if (pA.parryUnlocked)
        {
            parryboughtText.text = "Parry is unlocked";
            //parryButtonText.text = "Equip Parry";
        }
        else
        {
            parryboughtText.text = "Parry is not unlocked : " + parryCost;
            //parryButtonText.text = "Buy Parry";
        }

        if (!AbilityManager.checkIfFileExists)
        {

            dashboughttext.text = "Dash is not unlocked : " + dashCost;
            //dashButtonText.text = "Buy Dash";
            parryboughtText.text = "Parry is not unlocked : " + parryCost;
            //parryButtonText.text = "Buy Parry";
            equippedtext.text = "Nothing is equipped";
        }
        currencyText.text = "time: " + string.Format("{0:0.00}", LevelStatsManager.totalTimeValue);
    }
    public void buyDashButton()
    {
        if (pA.dashUnlocked)
        {
            //allow the player to just equip
            pA.equippedAbility = Abilities.AbilityType.dash;
        }
        else if(LevelStatsManager.canBuyLevel(dashCost))
        {
            //if the player has enough money,
            //then buy parry
            LevelStatsManager.buyLevel(dashCost);
            Debug.Log("Bought dash");

            pA.dashUnlocked = true;
            pA.equippedAbility = Abilities.AbilityType.dash;
            AbilityManager.instance.SaveAbilities(pA);
            dashtutorialobject.SetActive(true);
        }
        else if(!LevelStatsManager.canBuyLevel(dashCost))
        {
            //if the player does not have enough money
            Debug.Log("Not enough money");
        }
        updateText();
    }
    
    public void buyParryButton()
    {
        if (pA.parryUnlocked)
        {
            //allow the player to just equip
            pA.equippedAbility = Abilities.AbilityType.parry;
        }
        else if (LevelStatsManager.canBuyLevel(parryCost))
        {
            //if the player has enough money,
            //then buy parry
            LevelStatsManager.buyLevel(parryCost);
            Debug.Log("Bought parry");
            pA.parryUnlocked = true;
            pA.equippedAbility = Abilities.AbilityType.parry;
            AbilityManager.instance.SaveAbilities(pA);
            parrytutorialobject.SetActive(true);
        }
        else if (!LevelStatsManager.canBuyLevel(dashCost))
        {
            //if the player does not have enough money
            Debug.Log("Not enough money");
        }
        updateText();
    }
    public void equipParry()
    {
        Abilities potAb = AbilityManager.instance.savedAbilities;
        potAb.equippedAbility = Abilities.AbilityType.parry;
        AbilityManager.instance.SaveAbilities(potAb);
    }
}
