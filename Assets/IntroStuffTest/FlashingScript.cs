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

    public bool startCountDown = false;
    public float countDown = 3f;
    public float rateHorizontal = 0.1f;

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
                startCountDown = true;
            }

            if (startCountDown)
            {
                countDown -= Time.deltaTime;
            }

            if (countDown <= 0 && alpha.a >= 0)
            {
                //alpha.a -= rateDeInAlpha * (transform.position.y - barForTransparent);
                alpha.a -= rateHorizontal*Time.deltaTime;
            }
        }
        else if(horizontal)
        {
            if (transform.position.x >= barForTransparent)
            {
                startCountDown = true;
            }

            if (startCountDown)
            {
                countDown -= Time.deltaTime;
            }

            if (countDown <= 0 && alpha.a >= 0)
            {
                alpha.a -= rateHorizontal*Time.deltaTime;
            }
        }

        myRend.color = alpha;
    }
}