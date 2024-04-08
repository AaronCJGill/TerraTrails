using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class FlashingScript : MonoBehaviour
{
    SpriteRenderer myRend;

    public float rateDeInAlpha = 1;

    public float barForTransparent = 20;

    // Start is called before the first frame update
    void Start()
    {
        myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color alpha = myRend.color;

        if (transform.position.y >= barForTransparent)
        {
            if(alpha.a >= 0)
            {
                alpha.a -= rateDeInAlpha * (transform.position.y - barForTransparent);
            }
        }

        myRend.color = alpha;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
    }
}