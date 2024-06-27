using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Fade : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] Animation anim;

    private void Start()
    {
        dialogueRunner.AddCommandHandler<string>("fade", FadeTransition);
    }

    public IEnumerator FadeTransition(string fadeClip)
    {
        anim.clip = anim.GetClip(fadeClip);

        anim.Play();
        Debug.Log("Playing: " + anim.clip);
        yield return new WaitUntil(() => !anim.isPlaying);
    }
}
