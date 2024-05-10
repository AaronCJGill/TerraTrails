using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonEventLogic : MonoBehaviour
{
    Image image;
    Animator animator;
    Vector3 originalScale;
    Vector2 originalPosition;
    Vector4 originalPadding;
    public float scaleValue;
    public float offsetValue;

    AudioSource _as;
    AudioClip _ac;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        animator = gameObject.GetComponent<Animator>();
        originalScale = transform.localScale;
        originalPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        originalPadding = image.raycastPadding;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hoveredButton()
    {
        LeanTween.scale(gameObject, new Vector3(scaleValue, scaleValue, 1f), 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveX((RectTransform)transform, originalPosition.x + offsetValue, 0.5f).setEase(LeanTweenType.easeOutQuad);
        image.raycastPadding = new Vector4(image.raycastPadding.x - offsetValue, image.raycastPadding.y, image.raycastPadding.z, image.raycastPadding.w);
        if(animator != null)
        {
            animator.SetTrigger("startHover");
        }
    }
    public void leftButton()
    {
        LeanTween.scale(gameObject, originalScale, 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveX((RectTransform)transform, originalPosition.x, 0.5f).setEase(LeanTweenType.easeOutQuad);
        image.raycastPadding = originalPadding;
        if (animator != null)
        {
            animator.SetTrigger("endHover");
        }

    }

    public void clickFx()
    {
        Debug.Log("CLIK");
        var par = transform.parent.transform.parent;
        var uiMngr = par.GetComponent<uiManager>();
        AudioClip snClick = uiMngr.snClick;
        _as = par.GetComponent<AudioSource>();
        _as.pitch = 1.5f;
        //_as.volume = 1;
        _as.PlayOneShot(snClick, .8f);
    }
}
