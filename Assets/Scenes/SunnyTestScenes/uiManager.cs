using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("GameObject References")]
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject topShutter;
    public GameObject bottomShutter;
    public GameObject gameOverPanel;
    public GameObject retryButton;
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
    public Vector2 retryButtonGoal;
    public Vector2 detailBloodGoal;

    void Start()
    {
        LeanTween.init(800);
        gameOverPanel.SetActive(false);
        leftPanelStart = leftPanel.GetComponent<RectTransform>().anchoredPosition;
        rightPanelStart = rightPanel.GetComponent<RectTransform>().anchoredPosition;
        topShutterStart = topShutter.GetComponent<RectTransform>().anchoredPosition;
        bottomShutterStart = bottomShutter.GetComponent<RectTransform>().anchoredPosition;
        gameOverStart = gameOverPanel.GetComponent<RectTransform>().anchoredPosition;
        detailBloodStart = detailBlood.GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
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
}
