using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiManager : MonoBehaviour
{
    private bool isPlayed;

    [Header("GameObject References")]
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject topShutter;
    public GameObject bottomShutter;
    public GameObject deathScreen;

    [Header("Goalposts")]
    public float transitionPanelTime;
    public float transitionShutterTime;
    public Vector3 leftPanelGoal;
    public Vector3 rightPanelGoal;
    public Vector3 topShutterGoal;
    public Vector3 bottomShutterGoal;

    void Start()
    {

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
            LeanTween.moveLocal(topShutter, new Vector3(topShutterGoal.x, topShutterGoal.y, topShutterGoal.z), transitionShutterTime).setEase(LeanTweenType.easeOutBounce);
            LeanTween.moveLocal(bottomShutter, new Vector3(bottomShutterGoal.x, bottomShutterGoal.y, bottomShutterGoal.z), transitionShutterTime).setEase(LeanTweenType.easeOutBounce);
            isPlayed = true;
        }
    }
}
