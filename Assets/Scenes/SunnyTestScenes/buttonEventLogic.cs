using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonEventLogic : MonoBehaviour
{
    Animator animator;
    Vector3 originalScale;
    Vector2 originalPosition;
    public float scaleValue;
    public float offsetValue;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        originalScale = transform.localScale;
        originalPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void hoveredButton()
    {
        LeanTween.scale(gameObject, new Vector3(scaleValue, scaleValue, 1f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveX((RectTransform)transform, originalPosition.x + offsetValue, 0.5f).setEase(LeanTweenType.easeOutQuad);
        animator.SetTrigger("startHover");
    }
    public void leftButton()
    {
        LeanTween.scale(gameObject, originalScale, 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveX((RectTransform)transform, originalPosition.x, 0.5f).setEase(LeanTweenType.easeOutQuad);
        animator.SetTrigger("endHover");
    }
}
