using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCircle : MonoBehaviour
{
    //grows in size to radius, disappears over that time, and destroys self at end of time
    private float finalR = 2;
    private float startR = 1;
    private float time = 0;
    private float destroyTime = 0.5f;
    private float r = 0.1f;
    SpriteRenderer sr;
    Color startColor = Color.white;
    Color finalColor;

    public void init(float r, float destroyTime = 0.5f)
    {
        sr = GetComponent<SpriteRenderer>();
        finalColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        
        finalR = r;
        this.destroyTime = destroyTime;

        StartCoroutine(growQuickly());
    }
    IEnumerator growQuickly()
    {

        float time = 0;
        float finalOpacity = 0;
        
        while (time < destroyTime)
        {
            //currentOpacity = Mathf.Lerp(startOpacity, finalOpacity, time / destroyTime);
            time += Time.deltaTime;
            
            //do alpha shifting 
            sr.color = new Color(startColor.r, startColor.g, startColor.b, EasingFunction.EaseInCubic(startColor.a, finalColor.a, time / destroyTime));

            // do size shifting
            transform.localScale = new Vector3(EasingFunction.EaseInCubic(startR, finalR, time / destroyTime), EasingFunction.EaseInCubic(startR, finalR, time / destroyTime), 1);
            r = EasingFunction.EaseInCubic(startR, finalR, time / destroyTime);
            yield return null;
        }

        sr.color = new Color(startColor.r, startColor.g, startColor.b, finalOpacity);
        Destroy(gameObject);
    }
    private void Update()
    {
        Collider2D[] g = Physics2D.OverlapCircleAll(transform.position, r);
        foreach (var item in g)
        {
            if(item.CompareTag("projectile"))
                Destroy(item.gameObject);
        }
    }
}
