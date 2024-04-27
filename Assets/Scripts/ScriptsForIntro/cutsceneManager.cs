using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutsceneManager : MonoBehaviour
{
    public GameObject test;
    bool isPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (test.activeSelf && !isPlayed)
        {
            LeanTween.move(test, new Vector3(-4f, 0, 0), 1f).setEase(LeanTweenType.easeOutBack);
            isPlayed = true;
        }
    }
}
