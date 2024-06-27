using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InspectObject : MonoBehaviour, IInteractable
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] string dialogueNode;
    [SerializeField] GameObject canvas;

    public void InteractLogic()
    {
        dialogueRunner.StartDialogue(dialogueNode);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInput>().SetIInstance(this);
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInput>().ClearIInstance();
            canvas.SetActive(false);
        }
    }
}
