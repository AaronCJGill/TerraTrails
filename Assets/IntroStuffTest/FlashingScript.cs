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

    public bool horizontal = false;
    public bool transparentStart = false;

    public bool startCountDown = false;
    public float countDown = 3f;

    // Start is called before the first frame update
    void Start()
    {
        myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Color alpha = myRend.color;

        if (!horizontal)
        {
            if (transform.position.y >= barForTransparent)
            {
                if(alpha.a >= 0)
                {
                    alpha.a -= rateDeInAlpha * (transform.position.y - barForTransparent);
                }
            }
        }
        else
        {
            if (transform.position.x >= barForTransparent)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    startCountDown = true;
                }

                if (startCountDown)
                {
                    countDown -= Time.deltaTime;
                }

                if(countDown <= 0)
                {
                    transparentStart = true;
                }

                if (transparentStart && alpha.a >= 0)
                {
                    alpha.a -= rateDeInAlpha * (transform.position.x - barForTransparent);
                }
            }
        }

        myRend.color = alpha;
    }
}