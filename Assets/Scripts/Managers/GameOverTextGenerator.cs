using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTextGenerator : MonoBehaviour
{
    [SerializeField]
    public TextAsset gameOverPhrases;

    [SerializeField]
    private List<string> GameOverStrings = new List<string>();

    public static GameOverTextGenerator instance;

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
        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("remakeStringList")]
    public void remakeStringList()
    {
        string[] s = gameOverPhrases.text.Split("\n");
        for (int i = 0; i < s.Length; i++)
        {
            GameOverStrings.Add(s[i]);
        }
    }

    public string generateString()
    {
        if (GameOverStrings[0] == null)
        {
            remakeStringList();
        }
        return GameOverStrings[Random.Range(0, GameOverStrings.Count)];
    }

}
