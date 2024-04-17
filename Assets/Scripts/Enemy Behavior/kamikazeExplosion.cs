using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kamikazeExplosion : MonoBehaviour
{
    [SerializeField]
    float explosionTime = 0.4f;
    float timer = 0;
    [SerializeField]
    float bounceBackTime = 0.1f;
    Vector3 startScale, endScale;


    private IEnumerator explode()
    {
        transform.localScale = startScale;
        while (timer < explosionTime)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, timer / explosionTime);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
        //grow smaller somewhat
        timer = 0;
        while (timer < explosionTime)
        {
            transform.localScale = Vector3.Lerp(endScale, endScale * 0.6f, timer / bounceBackTime);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        //transform.localScale = endScale * 0.6f;

    }
    public void init(float multiplier, float t)
    {
        float explosionTime = 0.4f;
        //startScale = Vector3.one;
        startScale = Vector3.one * 0.01f;
        endScale = Vector3.one * multiplier;
        //Invoke("startexplosion", delay);
        //startexplosion();
        StartCoroutine(explode());
    }
    float delay = 1f;
    private void startexplosion()
    {
        StartCoroutine(explode());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health.instance.takeDamage(1);
        }
    }
}
