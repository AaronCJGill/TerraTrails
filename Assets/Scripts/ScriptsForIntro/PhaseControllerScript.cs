using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseControllerScript : MonoBehaviour
{

    public float phaseNumber = 0f;

    public float setWaitTime = 2f;
    float waitTime;

    public float setloadingTime = 1f;
    float loadingTime;

    public float longerTime = 3f;
    public float shorterTime = 1.5f;

    public float scrollingTime = 3f;

    //Assests from Page#2
    public GameObject page2P1;
    public GameObject page2P2;
    public GameObject page2P3;
    public GameObject page2P4;
    public GameObject page2P5;
    public GameObject page2P6;
    public GameObject page2P7;

    //Assets from Page#3
    public GameObject page3P1;
    public GameObject page3P2;
    public GameObject page3P3;
    public GameObject page3P4_1;
    public GameObject page3P4_2;

    //Assets from Page#4
    public GameObject page4P1;
    public GameObject page4P2;

    //UI
    public GameObject space;

    //Check that if the image is played
    bool p2p1 = false;
    bool p2p2 = false;
    bool p2p3 = false;

    bool p2p5 = false;
    bool p2p6 = false;
    bool p2p7 = false;

    bool p3p1 = false;
    bool instruction = false;

    //Check that if the section of play is over
    bool p2s1 = false;
    bool p2s2 = false;

    CameraShake myCameraShake;

    // Start is called before the first frame update
    void Start()
    {
        //phaseNumber = 0;
        waitTime = setWaitTime;
        loadingTime = setloadingTime;

        myCameraShake = GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("phase" + phaseNumber);
        Debug.Log("waitTime =" + waitTime);
        Debug.Log("loadingTime =" + loadingTime);

        //allow player to completely skip the scene
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(2);
        }

        //Beginning time to wait for the animation
        if (phaseNumber == 0)
        {
            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                phaseNumber = 1;
                waitTime = setWaitTime;
            }
        }

        //Part1 Decrease in energy
        if (phaseNumber == 1)
        {
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
            else if (loadingTime <= 0 && !p3p1)
            {
                page3P1.SetActive(true);

                loadingTime = setloadingTime;
                p3p1 = true;
            }
            else if (loadingTime <= 0 && !instruction)
            {
                space.SetActive(true);

                loadingTime = setloadingTime;
                instruction = true;
            }


            if (p2p1 && p2p2 && p2p3 && p3p1 && instruction)
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
                    page3P1.SetActive(false);
                    space.SetActive(false);
                    instruction = false;

                    phaseNumber = 2;
                }
            }
        }

        //Part2 The Spaceship slows
        if (phaseNumber == 2)
        {
            page2P4.SetActive(true);

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                waitTime = longerTime;
                phaseNumber = 3;
            }

        }

        //part3 It runs out of fuel
        if (phaseNumber == 3)
        {
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
            else if (loadingTime <= 0 && !instruction)
            {
                //Vector3 newpos = new Vector3 (-5,-3,0);

                space.transform.position = new Vector3(-5, -3, 0);
                space.SetActive(true);

                loadingTime = setloadingTime;
                instruction = true;
            }

            if (p2p5 && p2p6 && p2p7 && instruction)
            {
                p2s2 = true;
            }

            if (loadingTime >= 0 && p2s2)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    page2P5.SetActive(false);
                    page2P6.SetActive(false);
                    page2P7.SetActive(false);
                    space.SetActive(false);
                    instruction = false;

                    phaseNumber = 4;
                }
            }
        }

        //Part4 It floats in the space
        if (phaseNumber == 4)
        {
            page3P2.SetActive(true);

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                //Vector3 newpos = new Vector3(5, -3.5f, 0);

                space.transform.position = new Vector3(5, -3.5f, 0);
                space.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    space.SetActive(false);
                    waitTime = setWaitTime;
                    phaseNumber =5;
                }
            }

            //if possible, could include a press to add to initiate bar
            //notification of hold
        }

        //Part5 Atmopshere entry
        if (phaseNumber == 5)
        {
            page3P3.SetActive(true);

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
                myCameraShake.Shake(0.1f);
            }

            if (waitTime <= 0)
            {
                waitTime = shorterTime;
                page3P3.SetActive(false);
                phaseNumber = 6;
            }
        }

        //Part6 Passing through the sky
        if (phaseNumber == 6)
        {
            page3P4_1.SetActive(true);
            page3P4_2.SetActive(true);

            myCameraShake.Transform.localEulerAngles = new Vector3(0, 0, 0);

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                waitTime = 1.5f*longerTime;
                page3P4_1.SetActive(false);
                page3P4_2.SetActive(false);
                phaseNumber = 7;
            }
        }

        //Part7 Scrolling from bottom to top
        if (phaseNumber == 7)
        {
            page4P1.SetActive(true);

            if (scrollingTime >= 0)
            {
                scrollingTime -= Time.deltaTime;
            }

            if (scrollingTime <= 0)
            {
                phaseNumber = 8;
            }
        }

        //Part8 Last scene
        if (phaseNumber == 8)
        {
            page4P2.SetActive(true);

            //Do we need a press space to start?

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                SceneManager.LoadScene(2);
            }

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    SceneManager.LoadScene(2);
            //}
        }

    }
}
