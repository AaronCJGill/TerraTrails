using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseControllerScript : MonoBehaviour
{

    public float phaseNumber = 0f;

    public float waitTime = 3f;

    public float newWaitTime = 3f;
    //public float setWaitTime = 3f;

    public float loadingTime = 1f;
    public float setloadingTime = 1f;

    public float scrollingTime = 3f;

    public GameObject page2P1;
    public GameObject page2P2;
    public GameObject page2P3;
    public GameObject page2P4;
    public GameObject page2P5;
    public GameObject page2P6;
    public GameObject page2P7;

    bool p2p1 = false;
    bool p2p2 = false;
    bool p2p3 = false;

    bool p2p5 = false;
    bool p2p6 = false;
    bool p2p7 = false;

    bool p2s1 = false;
    bool p2s2 = false;

    SpriteRenderer myRend;

    // Start is called before the first frame update
    void Start()
    {
        phaseNumber = 0;

        myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Beginning time to wait for the animation

        if(phaseNumber == 0)
        {
            Debug.Log("phase0");

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                phaseNumber = 1;
            }
        }

        //Fisrt Part
        if (phaseNumber == 1)
        {
            Debug.Log("phase1");

            if (loadingTime >= 0 && !p2s1)
            {
                loadingTime -= Time.deltaTime;
            }

            if (loadingTime <= 0 && !p2p1)
            {
                page2P1.SetActive(true);

                loadingTime = setloadingTime;
                p2p1 = true;
            }
            else if (loadingTime <= 0 && !p2p2)
            {
                page2P2.SetActive(true);

                loadingTime = setloadingTime;
                p2p2 = true;
            }
            else if (loadingTime <= 0 && !p2p3)
            {
                page2P3.SetActive(true);

                loadingTime = setloadingTime;
                p2p3 = true;
            }

            if(p2p1 && p2p2 && p2p3)
            {
                p2s1 = true;
            }

            if (loadingTime >= 0 && p2s1)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    page2P1.SetActive(false);
                    page2P2.SetActive(false);
                    page2P3.SetActive(false);

                    phaseNumber = 2;
                }
            }
        }

        //part2
        if(phaseNumber == 2)
        {
            Debug.Log("phase2");

            page2P4.SetActive(true);

            if (newWaitTime >= 0)
            {
                newWaitTime -= Time.deltaTime;
            }

            if (newWaitTime <= 0)
            {
                Debug.Log("ready for phase3");

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    phaseNumber = 3;
                }
            }

        }

        //part3
        if (phaseNumber == 3)
        {
            Debug.Log("phase3");

            if (loadingTime >= 0 && !p2s2)
            {
                loadingTime -= Time.deltaTime;
            }

            if (loadingTime <= 0 && !p2p5)
            {
                page2P5.SetActive(true);

                loadingTime = setloadingTime;
                p2p5 = true;
            }
            else if (loadingTime <= 0 && !p2p6)
            {
                page2P6.SetActive(true);

                loadingTime = setloadingTime;
                p2p6 = true;
            }
            else if (loadingTime <= 0 && !p2p7)
            {
                page2P7.SetActive(true);

                loadingTime = setloadingTime;
                p2p7 = true;
            }

            if (p2p5 && p2p6 && p2p7)
            {
                p2s2 = true;
            }

            if (loadingTime >= 0 && p2s2)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }

    }
}
