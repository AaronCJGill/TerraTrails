using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiManager : MonoBehaviour
{
    private bool isPlayed;
    //Starter Positions
    Vector3 leftPanelStart;
    Vector3 rightPanelStart;
    Vector3 topShutterStart;
    Vector3 bottomShutterStart;
    Vector3 deathScreenStart;

    [Header("GameObject References")]
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject topShutter;
    public GameObject bottomShutter;
    public GameObject deathScreen;
    [Header("Transition Times")]
    public float transitionPanelTime;
    public float transitionShutterTime;
    public float transitionDeathTime;
    [Header("Goalposts")]
    public Vector3 leftPanelGoal;
    public Vector3 rightPanelGoal;
    public Vector3 topShutterGoal;
    public Vector3 bottomShutterGoal;
    public Vector3 deathScreenGoal;

    void Start()
    {
        leftPanelStart = leftPanel.transform.localPosition;
        rightPanelStart = rightPanel.transform.localPosition;
        topShutterStart = topShutter.transform.localPosition;
        bottomShutterStart = bottomShutter.transform.localPosition;
        deathScreenStart = deathScreen.transform.localPosition;
    }

    void Update()
    {
        LeanTween.init(800);
        //If the player dies
        if (deathScreen.activeSelf && !isPlayed)
        {
            //Hide side panels
            LeanTween.moveLocal(leftPanel, new Vector3(leftPanelGoal.x, leftPanelGoal.y, leftPanelGoal.z), transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocal(rightPanel, new Vector3(rightPanelGoal.x, rightPanelGoal.y, rightPanelGoal.z), transitionPanelTime).setEase(LeanTweenType.easeOutCubic);
            //Collapse Shutter
            LeanTween.moveLocal(topShutter, new Vector3(topShutterGoal.x, topShutterGoal.y, topShutterGoal.z), transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
            LeanTween.moveLocal(bottomShutter, new Vector3(bottomShutterGoal.x, bottomShutterGoal.y, bottomShutterGoal.z), transitionShutterTime).setDelay(0.15f).setEase(LeanTweenType.easeOutSine);
            //Bring in death menu
            LeanTween.moveLocal(deathScreen, new Vector3(deathScreenGoal.x, deathScreenGoal.y, deathScreenGoal.z), transitionDeathTime).setDelay(0.5f).setEase(LeanTweenType.easeOutBack);
            isPlayed = true;
        }
        else if(!deathScreen.activeSelf && isPlayed){
            LeanTween.moveLocal(leftPanel, new Vector3(leftPanelStart.x, leftPanelStart.y, leftPanelStart.z), 1f);
            LeanTween.moveLocal(rightPanel, new Vector3(rightPanelStart.x, rightPanelStart.y, rightPanelStart.z), 1f);
            LeanTween.moveLocal(topShutter, new Vector3(topShutterStart.x, topShutterStart.y, topShutterStart.z), 1f);
            LeanTween.moveLocal(bottomShutter, new Vector3(bottomShutterStart.x, bottomShutterStart.y, bottomShutterStart.z), 1f);
            LeanTween.moveLocal(deathScreen, new Vector3(deathScreenStart.x, deathScreenStart.y, deathScreenStart.z), 1f);

            isPlayed = false;
        }
    }
}
