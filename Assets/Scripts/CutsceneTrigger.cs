using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] string node = "Discovery";
    
    private Collider player;
    private Collider col;
    private Transform endPos;
    private float velocity = 5f;

    private void Start()
    {
        col = GetComponent<Collider>();
        endPos = GetComponentInChildren<Transform>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.enabled = false;
            player = other;
            col.enabled = false;
            dialogueRunner.StartDialogue(node);
        }
    }

    private void FixedUpdate()
    {
        if (player != null && player.transform.position.x > endPos.position.x)
        {
            player.transform.position += Vector3.left * velocity * Time.deltaTime;
        } else if (player != null && !player.enabled)
        {
            player.enabled = true;
            Debug.Log("Player can move again");
            Destroy(gameObject);
        }
    }
}
