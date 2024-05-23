using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn.Unity;

public class BreathingMinigame : MonoBehaviour
{
    [Header("Breathing objects")]
    [SerializeField] GameObject breathingView;
    [SerializeField] Camera cam;
    [SerializeField] RectTransform breathingIndicator;
    [SerializeField] Button breatheButton;
    [SerializeField] TMP_Text text;

    [Header("Dialogue runner")]
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] string dialogueNode;

    private bool breathingIn = true;
    private bool breathingActive;
    private bool mouseOverButton;

    private float minSize = 200f;
    private float maxSize = 500f;
    private float breathInTime = 3f;
    private float sizeIncreasePerSecond;
    private float breathOutTime = 4f;
    private float sizeDecreasePerSecond;

    private float breathCycles;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.AddCommandHandler<bool>("breathing_minigame", StartBreathingMinigame);
        sizeIncreasePerSecond = (maxSize - minSize) / breathInTime;
        sizeDecreasePerSecond = (maxSize - minSize) / breathOutTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (breathingActive)
        {
            if (Input.GetMouseButton(0) && mouseOverButton && breathingIn)
            {
                float newSize = breathingIndicator.sizeDelta.x + (sizeIncreasePerSecond * Time.deltaTime);
                breathingIndicator.sizeDelta = new Vector2(newSize, newSize);
                cam.orthographicSize += breathOutTime / breathInTime  * Time.deltaTime;
            }
            else if (!Input.GetMouseButton(0) && !breathingIn)
            {
                float newSize = breathingIndicator.sizeDelta.x - (sizeDecreasePerSecond * Time.deltaTime);
                breathingIndicator.sizeDelta = new Vector2(newSize, newSize);
                cam.orthographicSize -= Time.deltaTime;
            }

            if (breathingIndicator.sizeDelta.x >= maxSize && breathingIn)
            {
                breathingIn = false;
                text.text = "Breathe out";
            }
            else if (breathingIndicator.sizeDelta.x <= minSize && !breathingIn)
            {
                breathingIn = true;
                text.text = "Breathe in";
                breathCycles++;
            }

            if(breathCycles >= 3)
            {
                StartBreathingMinigame(false);
                dialogueRunner.StartDialogue(dialogueNode);
            }
        }
    }

    public void OnHoverButton(bool hover)
    {
        mouseOverButton = hover;
    }

    public void StartBreathingMinigame(bool start)
    {
        breathingView.SetActive(start);
        breathingActive = start;
    }
}
