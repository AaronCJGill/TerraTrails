using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OptionsMenuManager : MonoBehaviour
{
    [Header("Leantween")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject BG;
    public Vector3 menuGoalOffPosition;
    public Vector3 optionsMenuOffPosition;
    public float transitionTime;
    [Header("Button States")]
    public GameObject audioTab;
    public GameObject videoTab;
    public Sprite audioDefault;
    public Sprite audioSelected;
    public Sprite videoDefault;
    public Sprite videoSelected;

    private Vector2 BGPositionStart;

    void Start()
    {
        BGPositionStart = BG.GetComponent<RectTransform>().anchoredPosition;
    }

    public void openOptions()
    {
        LeanTween.move(mainMenu.GetComponent<RectTransform>(), menuGoalOffPosition, 1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(optionsMenu.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f), transitionTime).setEase(LeanTweenType.easeOutBack).setDelay(0.25f);
        LeanTween.move(BG.GetComponent<RectTransform>(), new Vector3(BGPositionStart.x - 30f, BGPositionStart.y, 0f), 1.25f).setEase(LeanTweenType.easeOutQuad);
    }

    public void closeOptions()
    {
        LeanTween.move(mainMenu.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f), transitionTime).setEase(LeanTweenType.easeOutBack).setDelay(0.25f); ;
        LeanTween.move(optionsMenu.GetComponent<RectTransform>(), optionsMenuOffPosition, 1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.move(BG.GetComponent<RectTransform>(), BGPositionStart, 1.25f).setEase(LeanTweenType.easeOutQuad);
    }

    public void viewingAudio()
    {
        //Change sprites
        videoTab.GetComponent<Image>().sprite = videoDefault;
        audioTab.GetComponent<Image>().sprite = audioSelected;
    }
    public void viewingVideo()
    {
        videoTab.GetComponent<Image>().sprite = videoSelected;
        audioTab.GetComponent<Image>().sprite = audioDefault;
    }
}
