using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObject : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer image;
    [SerializeField]
    float timerTotal = 2;
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > timerTotal)
        {
            timer = 0;
            image.enabled = !image.enabled;
        }
    }
}
