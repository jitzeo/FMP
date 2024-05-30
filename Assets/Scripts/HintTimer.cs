using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class HintTimer : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;

    private bool stopHint;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner.AddCommandHandler<string, float>("hint", SetHintTimer);
        dialogueRunner.AddCommandHandler("stop_hint", StopHint);
    }

    public void SetHintTimer(string node, float waitTime)
    {
        StartCoroutine(Hint(node, waitTime));
    }

    public void StopHint()
    {
        stopHint = true;
    }

    public IEnumerator Hint(string node, float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        yield return new WaitUntil(() => !dialogueRunner.IsDialogueRunning);
        if (!stopHint)
        {
            dialogueRunner.StartDialogue(node);
        }
    }
}
