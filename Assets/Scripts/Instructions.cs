using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class Instructions : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] GameObject instructionsView;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] TMP_Text text;

    private bool fadeOutInstructions;
    private float fadeTime = 0.5f;
    private float t;

    private string[] instructionKeys;
    private bool instructionKeyPressed = true;

    private void Start()
    {
        dialogueRunner.AddCommandHandler<string, string, string>("instructions", ShowInstructions);
    }

    private void Update()
    {
        if (!instructionKeyPressed)
        {
            foreach (string key in instructionKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    instructionKeyPressed = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (fadeOutInstructions)
        {
            canvas.alpha = Mathf.Lerp(1, 0, t * Time.deltaTime);
            t += 1 / fadeTime;
        }
    }

    public IEnumerator ShowInstructions(string instructions, string controls, string node = "none")
    {
        instructionsView.SetActive(true);
        canvas.alpha = 1f;
        text.text = instructions;

        if(controls == "left")
        {
            instructionKeys = new string[] {"left", "a"};
        } 
        else if (controls == "rotate")
        {
            instructionKeys = new string[] { "q", "e" };
        } 
        else if(controls == "shift")
        {
            instructionKeys = new string[] { "left shift", "right shift" };
        }
        instructionKeyPressed = false;
        yield return new WaitUntil(() => instructionKeyPressed);

        yield return new WaitForSecondsRealtime(0.2f);
        fadeOutInstructions = true;

        yield return new WaitUntil(() => canvas.alpha <= 0f);
        fadeOutInstructions = false;
        t = 0;
        instructionsView.SetActive(false);

        if(node != "none")
        {
            dialogueRunner.StartDialogue(node);
        }
    }
}
