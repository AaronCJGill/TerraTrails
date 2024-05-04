using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AbilityManager : MonoBehaviour
{
    //is able to save the currenlty active ability that the player has
    public Abilities savedAbilities;
    public static AbilityManager instance;

    public static string settingsName = "AbilitySettings";
    public static string directory = "/SettingsData/";
    public static bool checkIfFileExists
    {
        //checks if save file exists
        get { return File.Exists(Application.persistentDataPath + directory + settingsName + ".txt"); }
    }
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

            loadAbilities();


        }
    }
    public void resetAbilities()
    {
        Abilities ab = new Abilities(Abilities.AbilityType.nothing);
        savedAbilities = ab;
        SaveAbilities(ab);
        Debug.Log("Created new abilities " + savedAbilities.equippedAbility);
    }
    public void SaveAbilities(Abilities potentialAbilities)
    {
        savedAbilities = potentialAbilities;
        string dir = Application.persistentDataPath + directory;
        string json;

        string filePath = dir + settingsName + ".txt";
        json = JsonUtility.ToJson(savedAbilities);
        File.WriteAllText(filePath, json);
        Debug.Log("Abilities Saved");


    }

    public Abilities loadAbilities()
    {
        string dir = Application.persistentDataPath + directory;
        string filePath = dir + settingsName + ".txt";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            savedAbilities = JsonUtility.FromJson<Abilities>(json);

            return savedAbilities;
        }
        else
        {
            Abilities ab = new Abilities(Abilities.AbilityType.nothing);
            savedAbilities = ab;
            SaveAbilities(ab);
            Debug.Log("Created new abilities " + savedAbilities.equippedAbility);
            return ab;
        }
    }
    public static void setPlayerAbility()
    {
        if (instance.savedAbilities.equippedAbility == Abilities.AbilityType.dash)
        {
            PlayerMovement.instance.gameObject.AddComponent<PlayerDash>();
        }
        else if(instance.savedAbilities.equippedAbility == Abilities.AbilityType.parry)
        {
            PlayerMovement.instance.gameObject.AddComponent<Parry>();
        }
    }
    public static void deleteFilePath()
    {
        string dir = Application.persistentDataPath + directory;
        string filePath = dir + settingsName + ".txt";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
    public static void activatePowerup()
    {
        
        switch (instance.savedAbilities.equippedAbility)
        {
            case Abilities.AbilityType.nothing:
                break;
            case Abilities.AbilityType.dash:
                PlayerMovement.instance.gameObject.AddComponent<PlayerDash>();
                break;
            case Abilities.AbilityType.parry:
                PlayerMovement.instance.gameObject.AddComponent<Parry>();
                break;
            default:
                break;
        }
    }
}

public struct Abilities
{
    public AbilityType equippedAbility;
    //sorry this is going to be really ugly 
    public bool parryUnlocked;
    public bool dashUnlocked;
    public enum AbilityType
    {
        nothing,
        dash,
        parry,

    }
    public Abilities(AbilityType t)
    {
        equippedAbility = t;
        parryUnlocked = false;
        dashUnlocked = false;
    }
}

