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
        LeanTween.init(800);

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
                endGame(true);
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
    public void endGame(bool hasDied = false)
    {
        //Player passes the game
        if (!hasDied)
        {
            //Hide side panels
            LeanTween.move(leftPanel.GetComponent<RectTransform>(), leftPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
            LeanTween.move(rightPanel.GetComponent<RectTransform>(), rightPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
            //Collapse Shutter
            LeanTween.move(topShutter.GetComponent<RectTransform>(), new Vector3(topShutterGoal.x, topShutterGoal.y, -15f), transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
            LeanTween.move(bottomShutter.GetComponent<RectTransform>(), bottomShutterGoal, transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
            //Bring in death menu
            LeanTween.move(gameOverPanel.GetComponent<RectTransform>(), gameOverGoal, transitionGameOverTime).setDelay(0.5f).setEase(LeanTweenType.easeOutBack);
            //LeanTween.move(retryButton.GetComponent<RectTransform>(), retryButtonGoal, transitionGameOverTime).setDelay(1f).setEase(LeanTweenType.easeOutBack);
        }
        //Player dies
        else
        {
            //Hide side panels
            LeanTween.move(leftPanel.GetComponent<RectTransform>(), leftPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
            LeanTween.move(rightPanel.GetComponent<RectTransform>(), rightPanelGoal, transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
            //Collapse Shutter
            LeanTween.move(topShutter.GetComponent<RectTransform>(), new Vector3(topShutterGoal.x, topShutterGoal.y, -15f), transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
            LeanTween.move(bottomShutter.GetComponent<RectTransform>(), bottomShutterGoal, transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
            //Bring in death menu
            LeanTween.move(gameOverPanel.GetComponent<RectTransform>(), gameOverGoal, transitionGameOverTime).setDelay(0.5f).setEase(LeanTweenType.easeOutBack);
            //LeanTween.move(retryButton.GetComponent<RectTransform>(), retryButtonGoal, transitionGameOverTime).setDelay(1f).setEase(LeanTweenType.easeOutCubic);
            //Move in additional effects
            LeanTween.move(detailBlood.GetComponent<RectTransform>(), detailBloodGoal, 3.25f).setDelay(1f).setEase(LeanTweenType.easeOutCirc);
        }

    }

    public void CloseScene(int sceneID = 0)
    {
        transitionElement.SetActive(true);
        transitionElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000f, 0f, 0f);
        LeanTween.cancel(transitionElement);
        LeanTween.moveX(transitionElement.GetComponent<RectTransform>(), 0f, 1f).setDelay(0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            SceneManager.LoadScene(sceneID);
        });
    }
}
