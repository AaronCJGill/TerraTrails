using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    //is able to save the currenlty active ability that the player has


}

public struct Abilities
{
    public AbilityType[] unlockedAbilities;
    public AbilityType equippedAbility;

    public enum AbilityType
    {
        nothing,
        dash,
        shield,

    }

    public Abilities(AbilityType[] unlocked)
    {
        equippedAbility = unlocked[0];
        unlockedAbilities = unlocked;
    }
    public Abilities(AbilityType t)
    {
        equippedAbility = t;
        AbilityType[] unlock = new AbilityType[1];
        unlock[0] = t;
        unlockedAbilities = unlock;
    }


}

