using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class detectionCircle : MonoBehaviour
{
    
    CircleCollider2D c;
    [SerializeField]
    bool detectionActive = true;
    SpriteRenderer sr;
    kamikazeBehavior parent;
    //should start at its brightest color
    Color startColor;
    Color endColor;//is just a third of alpha value
    [SerializeField]
    Color flashColor;
    private void Awake()
    {
        c = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
        endColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0);

    }

    public void init(float radius, kamikazeBehavior par)
    {
        //c.radius = radius / 2;
        parent = par;
        detectionActive = true;
        transform.localScale = Vector3.one * radius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("playerfounds");
            if (detectionActive)
            {
                //Debug.Log("player detected");
                parent.detectPlayer();
                sr.enabled = false;
                detectionActive = false;
            }
        }
    }
    public void dormantPeriod()
    {
        //anim of it 
        detectionActive = true;
    }

    public void cooldownPeriod(float t)
    {
        //Debug.Log("CooldownPeriod" + t);
        //sr.enabled = true;
        StartCoroutine(cooldownroutine(t - 0.45f));
    }
    IEnumerator cooldownroutine(float t)
    {
        float timer = 0;
        sr.color = endColor;
        while (timer < t)
        {
            sr.color = Color.Lerp(endColor, startColor, timer / t);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = startColor;

        //flash
        timer = 0;
        sr.color = startColor;
        while (timer < 0.25f)
        {
            sr.color = Color.Lerp(startColor, flashColor, timer / 0.25f);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = flashColor;
        timer = 0;
        while (timer < 0.2)
        {
            sr.color = Color.Lerp(flashColor, startColor, timer / 0.2f);
            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = startColor;


    }
    //control blinking of the radius sprite

}
