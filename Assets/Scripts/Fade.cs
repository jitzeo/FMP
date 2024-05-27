using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Fade : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    private Animation anim;
    //[SerializeField] List<AnimationClip> clipList;

    private void Start()
    {
        dialogueRunner.AddCommandHandler<string>("fade", FadeTransition);
        
        anim = GetComponent<Animation>();
    }

    public IEnumerator FadeTransition(string fadeClip)
    {
        anim.clip = anim.GetClip(fadeClip);

        anim.Play();
        Debug.Log("Playing: " + anim.clip);
        yield return new WaitUntil(() => !anim.isPlaying);
    }
}
