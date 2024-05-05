using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tabButtonLogic : MonoBehaviour
{
    Image image;
    Vector3 originalScale;
    Vector2 originalPosition;
    Vector4 originalPadding;
    public float scaleValue;
    public float offsetValue;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
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
        LeanTween.moveY((RectTransform)transform, originalPosition.y + offsetValue, 0.5f).setEase(LeanTweenType.easeOutQuad);
        image.raycastPadding = new Vector4(image.raycastPadding.x, image.raycastPadding.y - offsetValue, image.raycastPadding.z, image.raycastPadding.w);
    }

    public void leftButton()
    {
        LeanTween.scale(gameObject, originalScale, 0.5f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveY((RectTransform)transform, originalPosition.y, 0.5f).setEase(LeanTweenType.easeOutQuad);
        image.raycastPadding = originalPadding;
    }

    public void clickFx()
    {
        Debug.Log("CLIK");
        var par = transform.parent.transform.parent.transform.parent;
        var uiMngr = par.GetComponent<uiManager>();
        AudioClip snClick = uiMngr.snClick;
        var _as = par.GetComponent<AudioSource>();
        _as.pitch = 1.5f;
        _as.PlayOneShot(snClick, .6f);
    }

}
