using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTextGenerator : MonoBehaviour
{
    [SerializeField]
    List<string> GameOverStrings = new List<string>();

    public string generateString()
    {
        return GameOverStrings[Random.Range(0, GameOverStrings.Count)];
    }

}
