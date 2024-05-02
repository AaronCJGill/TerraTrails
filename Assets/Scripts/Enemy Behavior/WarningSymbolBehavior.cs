using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSymbolBehavior : MonoBehaviour
{


    [SerializeField]
    SpriteRenderer sr;
    [SerializeField]
    float destroyTime = 2f;//waittime /2
    /*
    Color startColor = Color.white;
    Color finalColor;
    // Start is called before the first frame update
    
    private void Start()
    {
        startColor = sr.color;
        finalColor = new Color(startColor.r, startColor.g, startColor.b, 0);
    }

    public void init(float despawnWhen)
    {
        destroyTime = despawnWhen;
        StartCoroutine(behavior());
    }
    public void init(float despawnWhen, bool isLarge = false)
    {
        if (isLarge)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        destroyTime = despawnWhen;
        StartCoroutine(behavior());
    }

    IEnumerator behavior()
    {
        //change opacity overtime or trigger animation
        float startOpacity = 255;
        float currentOpacity = 255;
        float finalOpacity = 0;
        float time = 0;

        while (time < destroyTime)
        {

            
            //currentOpacity = Mathf.Lerp(startOpacity, finalOpacity, time / destroyTime);
            time += Time.deltaTime;
            //sr.color = Color.Lerp(startColor, finalColor, time / destroyTime);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, EasingFunction.EaseInCubic(startColor.a, finalColor.a, time / destroyTime));
            //sr.color = new Color(startColor.r, startColor.g, startColor.b, currentOpacity);
            //Debug.Log(currentOpacity);
            yield return null;

        }
        currentOpacity = finalOpacity;
        //sr.color = new Color(startColor.r, startColor.g, startColor.b, currentOpacity);
        sr.color = finalColor;
        Destroy(gameObject, 1);
    }
    */

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
