using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class cutsceneManager : MonoBehaviour
{

    public float phaseNumber = 0f;

    public float setWaitTime = 2f;
    float waitTime;

    public float setloadingTime = 1f;
    float loadingTime;

    public float longerTime = 3f;
    public float shorterTime = 1.5f;

    public float scrollingTime = 3f;

    public float rateOfDecrease = 0.1f;

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
    public GameObject page3P2_1;
    public GameObject page3P2_2;
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
    bool p2p4 = false;
    bool p2p5 = false;
    bool p2p6 = false;
    bool p2p7 = false;

    bool p3p1 = false;
    bool p3p2 = false;
    bool p3p3 = false;
    bool p3p4 = false;
    bool p4p1 = false;
    bool instruction = false;

    //Check that if the section of play is over
    bool p2s1 = false;
    bool p2s2 = false;
    bool p3s1 = false;

    CameraShake myCameraShake;

    //goal for the movement of the images
    public Vector2 goal0 = new Vector2(1, 2);
    public Vector2 goal1 = new Vector2(1, 2);
    public Vector2 goal2 = new Vector2(1, 2);
    public Vector2 goal3 = new Vector2(1, 2);
    public Vector2 goal4;
    public Vector2 goal5;
    public Vector2 goal6;

    [Header("Audio")]
    public AudioClip snScene1;
    public AudioClip snScene2;
    public AudioClip snScene3;
    public AudioClip snScene4;
    public AudioClip snPopup;
    public AudioClip snNextScene;
    public AudioClip snLowFuel;
    public AudioClip snEngineLoop;

    public AudioSource as1;
    public AudioSource as2;
    public AudioSource as3;

    // Start is called before the first frame update
    void Start()
    {
        //phaseNumber = 0;
        waitTime = setWaitTime;
        loadingTime = setloadingTime;

        as2.PlayOneShot(snScene1, 1);

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
                waitTime = setWaitTime + 2f;
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
                as1.PlayOneShot(snPopup, 1);
            }
            else if (loadingTime <= 0 && !p2p2)
            {
                page2P2.SetActive(true);

                loadingTime = setloadingTime;
                p2p2 = true;
                as1.PlayOneShot(snPopup, 1);
            }
            else if (loadingTime <= 0 && !p2p3)
            {
                page2P3.SetActive(true);

                loadingTime = setloadingTime;
                p2p3 = true;
                as1.PlayOneShot(snPopup, 1);
                as2.PlayOneShot(snLowFuel, 1);
            }
            else if (loadingTime <= 0 && !p3p1)
            {
                page3P1.SetActive(true);

                loadingTime = setloadingTime;
                p3p1 = true;
                as1.PlayOneShot(snPopup, 1);
            }
            else if (loadingTime <= 0 && !instruction)
            {
                space.SetActive(true);

                loadingTime = setloadingTime;
                instruction = true;
                as1.PlayOneShot(snPopup, 1);
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

                    as1.PlayOneShot(snScene2, 1);
                    as2.PlayOneShot(snNextScene, 1);
                    as3.Stop();

                    phaseNumber = 2;
                }
            }
        }

        //Part2 The Spaceship slows
        if (phaseNumber == 2)
        {
            page2P4.SetActive(true);

            Color alpha = page2P4.GetComponent<RawImage>().color;

            if (page2P4 && !p2p4)
            {
                LeanTween.move(page2P4.GetComponent<RectTransform>(), goal0, waitTime).setEase(LeanTweenType.easeOutCubic);
                p2p4 = true;
            }

            if(waitTime <= 1 && alpha.a >= 0)
            {
                alpha.a -= Time.deltaTime;
                page2P4.GetComponent<RawImage>().color = alpha;
            }

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                waitTime = longerTime;
                page2P4.SetActive(false);
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
                as2.PlayOneShot(snPopup, 1);
            }
            else if (loadingTime <= 0 && !p2p6)
            {
                page2P6.SetActive(true);

                loadingTime = setloadingTime;
                p2p6 = true;
                as2.PlayOneShot(snPopup, 1);
            }
            else if (loadingTime <= 0 && !p2p7)
            {
                page2P7.SetActive(true);

                loadingTime = setloadingTime;
                p2p7 = true;
                as2.PlayOneShot(snPopup, 1);
            }
            else if (loadingTime <= 0 && !instruction)
            {
                LeanTween.move(space.GetComponent<RectTransform>(), goal1, 0).setEase(LeanTweenType.easeOutBack);
                space.SetActive(true);

                loadingTime = setloadingTime;
                instruction = true;
                as2.PlayOneShot(snPopup, 1);
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

                    as1.PlayOneShot(snScene3, 1);
                    as2.PlayOneShot(snNextScene, 1);

                    phaseNumber = 4;
                }
            }
        }

        //Part4 It floats in the space
        if (phaseNumber == 4)
        {
            page3P2_1.SetActive(true);
            page3P2_2.SetActive(true);

            Color alpha = page3P2_1.GetComponent<RawImage>().color;

            if (waitTime <= 1.5 && alpha.a >= 0)
            {
                alpha.a -= Time.deltaTime;
                page3P2_1.GetComponent<RawImage>().color = alpha;
            }

            Color beta = page3P2_2.GetComponent<RawImage>().color;

            if (waitTime <= 1.5 && beta.a >= 0)
            {
                beta.a -= Time.deltaTime;
                page3P2_2.GetComponent<RawImage>().color = beta;
            }

            if (page3P2_2 && !p3p2)
            {
                LeanTween.move(page3P2_2.GetComponent<RectTransform>(), goal6, 0.5f).setEase(LeanTweenType.easeOutSine);
                p3p2 = true;
            }

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                if(!p3s1)
                {
                    LeanTween.move(space.GetComponent<RectTransform>(), goal2, 0).setEase(LeanTweenType.easeOutBack);
                    space.SetActive(true);
                    p3s1 = true;
                    as2.PlayOneShot(snPopup, 1);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    space.SetActive(false);
                    waitTime = setWaitTime;
                    page3P2_1.SetActive(false);
                    page3P2_2.SetActive(false);
                    phaseNumber = 5;

                    as1.PlayOneShot(snScene4, 1);
                    as2.PlayOneShot(snNextScene, 1);
                }
            }

            //if possible, could include a press to add to initiate bar
            //notification of hold
        }

        //Part5 Atmopshere entry
        if (phaseNumber == 5)
        {
            page3P3.SetActive(true);

            if (page3P3 && !p3p3)
            {
                LeanTween.move(page3P3.GetComponent<RectTransform>(), goal3, waitTime).setEase(LeanTweenType.easeInQuad);
                p3p3 = true;
            }

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

            Color alpha = page3P4_2.GetComponent<RawImage>().color;

            if (waitTime <= shorterTime && alpha.a <= 1)
            {
                alpha.a += Time.deltaTime;
                page3P4_2.GetComponent<RawImage>().color = alpha;
            }

            if (page3P4_2 && !p3p4)
            {
                LeanTween.move(page3P4_2.GetComponent<RectTransform>(), goal4, waitTime).setEase(LeanTweenType.easeInCubic);
                p3p4 = true;
            }

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                waitTime = longerTime;
                page3P4_1.SetActive(false);
                page3P4_2.SetActive(false);
                phaseNumber = 7;
            }
        }

        //Part7 Scrolling from bottom to top
        if (phaseNumber == 7)
        {
            page4P1.SetActive(true);

            Color alpha = page4P1.GetComponent<RawImage>().color;

            if (scrollingTime <= 1 && alpha.a >= 0)
            {
                alpha.a -= Time.deltaTime;
                page4P1.GetComponent<RawImage>().color = alpha;
            }

            if (page4P1 && !p4p1)
            {
                LeanTween.move(page4P1.GetComponent<RectTransform>(), goal5, scrollingTime).setEase(LeanTweenType.easeOutSine);
                p4p1 = true;
            }

            if (scrollingTime >= 0)
            {
                scrollingTime -= Time.deltaTime;
            }

            if (scrollingTime <= 0)
            {
                page4P1.SetActive(false);
                phaseNumber = 8;
            }
        }

        //Part8 Last scene
        if (phaseNumber == 8)
        {
            page4P2.SetActive(true);

            Color beta = page4P2.GetComponent<RawImage>().color;

            if (waitTime >= 4 && beta.a <= 1)
            {
                beta.a += Time.deltaTime;
                page4P2.GetComponent<RawImage>().color = beta;
            }

            Color alpha = page4P2.GetComponent<RawImage>().color;

            if (waitTime <= 2 && alpha.a >= 0)
            {
                alpha.a -= Time.deltaTime;
                page4P2.GetComponent<RawImage>().color = alpha;
            }

            if (waitTime >= 0)
            {
                waitTime -= Time.deltaTime;
            }

            if (waitTime <= 0)
            {
                SceneManager.LoadScene(2);
            }
        }

    }
}
