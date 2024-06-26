using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    private bool isPlayed;
    //Starter Positions
    Vector2 leftPanelStart;
    Vector2 rightPanelStart;
    Vector2 topShutterStart;
    Vector2 bottomShutterStart;
    Vector2 gameOverStart;
    Vector2 detailBloodStart;
    [Header("Configuration")]
    public bool isGameScene;
    public bool hasOpenTransition;
    [Header("GameObject References")]
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject topShutter;
    public GameObject bottomShutter;
    public GameObject gameOverPanel;
    public GameObject detailBlood;
    public GameObject passedPrompt;
    public GameObject nextButton;
    public GameObject mapButton;
    public GameObject retryButton;
    [Header("Transition Times")]
    public float transitionPanelTime;
    public float transitionShutterTime;
    public float transitionGameOverTime;
    [Header("Goalposts")]
    public Vector2 leftPanelGoal;
    public Vector2 rightPanelGoal;
    public Vector2 topShutterGoal;
    public Vector2 bottomShutterGoal;
    public Vector2 gameOverGoal;
    public Vector2 detailBloodGoal;
    [Header("Scene Transition")]
    public GameObject transitionElement;
    [Header("Audio")]
    public AudioClip snClick;
    public AudioClip snStart;
    public AudioClip snOptions;
    public AudioClip snNoSave;
    public AudioClip snSave;

    public AudioClip snScene1;

    AudioSource _as;
    AudioSource _bgS;

    public static uiManager instance;
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
     }

        void Start()
    {
        _as = GetComponent<AudioSource>();

        LeanTween.init(1600);

        if (isGameScene)
        {
            gameOverPanel.SetActive(false);
            //Grab Start Positions
            leftPanelStart = leftPanel.GetComponent<RectTransform>().anchoredPosition;
            rightPanelStart = rightPanel.GetComponent<RectTransform>().anchoredPosition;
            topShutterStart = topShutter.GetComponent<RectTransform>().anchoredPosition;
            bottomShutterStart = bottomShutter.GetComponent<RectTransform>().anchoredPosition;
            gameOverStart = gameOverPanel.GetComponent<RectTransform>().anchoredPosition;
            detailBloodStart = detailBlood.GetComponent<RectTransform>().anchoredPosition;
        }
        //Move transition element
        if(hasOpenTransition)
        {
            LeanTween.moveX(transitionElement.GetComponent<RectTransform>(), 970f, 1f).setDelay(0.5f).setEase(LeanTweenType.easeInCubic);
        }
        else
        {
            if(transitionElement != null)
            {
                transitionElement.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (isGameScene)
        {
            //If the player dies
            if (gameOverPanel.activeSelf && !isPlayed)
            {
                endGame();
                isPlayed = true;
            }
            else if (!gameOverPanel.activeSelf && isPlayed)
            {
                LeanTween.move(leftPanel.GetComponent<RectTransform>(), leftPanelStart, 1f);
                LeanTween.move(rightPanel.GetComponent<RectTransform>(), rightPanelStart, 1f);
                LeanTween.move(topShutter.GetComponent<RectTransform>(), topShutterStart, 1f);
                LeanTween.move(bottomShutter.GetComponent<RectTransform>(), bottomShutterStart, 1f);
                LeanTween.move(gameOverPanel.GetComponent<RectTransform>(), gameOverStart, 1f);

                isPlayed = false;
            }
        }
    }

    //There are additional decorative features if the player has died vs has passed the level, please set the bool accordingly. 
    public void endGame()
    {
        //Hide side panels
        LeanTween.move(leftPanel.GetComponent<RectTransform>(), leftPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(rightPanel.GetComponent<RectTransform>(), rightPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
        //Collapse Shutter
        LeanTween.move(topShutter.GetComponent<RectTransform>(), new Vector3(topShutterGoal.x, topShutterGoal.y, -15f), transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
        LeanTween.move(bottomShutter.GetComponent<RectTransform>(), bottomShutterGoal, transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
        Debug.Log("Game Over menu");
        //Bring in death menu
        gameOverPanel.SetActive(true);
        LeanTween.move(gameOverPanel.GetComponent<RectTransform>(), gameOverGoal, transitionGameOverTime).setDelay(0.5f).setEase(LeanTweenType.easeOutBack);
        
        //Player passes the game
        if (passedPrompt.activeSelf)
        {
            LeanTween.move(detailBlood.GetComponent<RectTransform>(), detailBloodStart, 3.25f).setDelay(1f).setEase(LeanTweenType.easeOutCirc);
            nextButton.SetActive(true);
            mapButton.SetActive(false);
            retryButton.SetActive(false);
        }
        //Player dies
        else
        {

            //Move in additional effects
            LeanTween.move(detailBlood.GetComponent<RectTransform>(), detailBloodGoal, 3.25f).setDelay(1f).setEase(LeanTweenType.easeOutCirc);
        }

    }

    public IEnumerator delayedEndGame()
    {
        yield return new WaitForSeconds(1.75f);
        //Hide side panels
        LeanTween.move(leftPanel.GetComponent<RectTransform>(), leftPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(rightPanel.GetComponent<RectTransform>(), rightPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
        //Collapse Shutter
        LeanTween.move(topShutter.GetComponent<RectTransform>(), new Vector3(topShutterGoal.x, topShutterGoal.y, -15f), transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
        LeanTween.move(bottomShutter.GetComponent<RectTransform>(), bottomShutterGoal, transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
        Debug.Log("Game Over menu");
        //Bring in death menu
        gameOverPanel.SetActive(true);
        LeanTween.move(gameOverPanel.GetComponent<RectTransform>(), gameOverGoal, transitionGameOverTime).setDelay(0.5f).setEase(LeanTweenType.easeOutBack);

        //Player passes the game
        if (passedPrompt.activeSelf)
        {
            LeanTween.move(detailBlood.GetComponent<RectTransform>(), detailBloodStart, 3.25f).setDelay(1f).setEase(LeanTweenType.easeOutCirc);
            nextButton.SetActive(true);
            mapButton.SetActive(false);
            retryButton.SetActive(false);
        }
        //Player dies
        else
        {

            //Move in additional effects
            LeanTween.move(detailBlood.GetComponent<RectTransform>(), detailBloodGoal, 3.25f).setDelay(1f).setEase(LeanTweenType.easeOutCirc);
        }
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        backgroundmusicmanager.instance.changeBackgroundMusic(backgroundmusicmanager.levtype.none);
        yield break;
    }

    public void CloseScene(int sceneID = 0)
    {
        transitionElement.SetActive(true);
        transitionElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000f, 0f, 0f);
        _as.pitch = 2f;
        _as.PlayOneShot(snClick, .8f);

        if (sceneID == 1)
        {
            StartCoroutine(StartFade(GameObject.Find("BackgroundMusicManager").GetComponent<AudioSource>(), 2.4f, 0));
        }

        LeanTween.cancel(transitionElement);
        LeanTween.moveX(transitionElement.GetComponent<RectTransform>(), 0f, 1f).setDelay(0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            SceneManager.LoadScene(sceneID);
        });
    }

    public void RestartScene()
    {
        _as.pitch = 2f;
        _as.PlayOneShot(snClick, .8f);

        transitionElement.SetActive(true);
        transitionElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000f, 0f, 0f);
        LeanTween.cancel(transitionElement);

        LeanTween.moveX(transitionElement.GetComponent<RectTransform>(), 0f, 1f).setDelay(0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
}
