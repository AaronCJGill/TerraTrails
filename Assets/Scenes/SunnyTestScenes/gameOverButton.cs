using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameOverButton : MonoBehaviour
{
    Vector3 originalScale;
    public float scaleValue;
    [Header("Hover Change")]
    public bool hasHoverSprite;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
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
        if (hasHoverSprite)
        {
            GetComponent<Image>().sprite = hoverSprite;
        }
    }
    public void leftButton()
    {
        LeanTween.scale(gameObject, originalScale, 0.5f).setEase(LeanTweenType.easeOutQuad);
        if (hasHoverSprite)
        {
            GetComponent<Image>().sprite = defaultSprite;
        }
    }
}
