using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 2f).setEase(LeanTweenType.easeOutCubic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
