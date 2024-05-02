using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleButtonLogic : MonoBehaviour
{
    Vector3 originalScale;
    public float scaleValue;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void hoveredButton()
    {
        LeanTween.scale(gameObject, new Vector3(scaleValue, scaleValue, 1f), 0.5f).setEase(LeanTweenType.easeOutQuad);
    }

    public void leftButton()
    {
        LeanTween.scale(gameObject, originalScale, 0.5f).setEase(LeanTweenType.easeOutQuad);
    }
}
