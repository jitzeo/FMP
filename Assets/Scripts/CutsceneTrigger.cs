using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] string node = "Discovery";
    
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            col.enabled = false;
            dialogueRunner.StartDialogue(node);
        }
    }
}
