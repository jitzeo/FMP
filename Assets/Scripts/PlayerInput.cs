using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Yarn.Unity;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] IslandManager islandManager;
    private float x;
    private float z;

    [SerializeField] Camera cam;
    [SerializeField] GameObject center;
    private float rotateSpeed = 50f;
    private float zoomSpeed = 50f;

    [SerializeField] Camera cam2D;

    [SerializeField] GameObject buttonInstructionsObject;
    [SerializeField] TMP_Text buttonInstructions;
    private string buttonInstructionsText;

    private bool freeze;
    private bool unlockedChangedPerspective;
    private bool changePerspectiveEnabled;

    private IInteractable interactableInstance;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        islandManager = GameObject.Find("IslandManager").GetComponent<IslandManager>();
        //center = GameObject.Find("Center");
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        buttonInstructionsText = "Press {0} {1} times to {2}";
    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        if (!freeze)
        {
            if (x != 0 || z != 0)
            {
                playerMovement.PlayerMove(x, z);
            }
        

            if (Input.GetKey(KeyCode.E))
            {
                cam.transform.RotateAround(center.transform.position, Vector3.down, rotateSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                cam.transform.RotateAround(center.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if(interactableInstance != null)
                {
                    interactableInstance.InteractLogic();
                }
            }

            if (Input.mouseScrollDelta.y > 0 && cam.orthographicSize > 5) {
                cam.orthographicSize -= zoomSpeed * Time.deltaTime;
            }

            if (Input.mouseScrollDelta.y < 0 && cam.orthographicSize < 12)
            {
                cam.orthographicSize += zoomSpeed * Time.deltaTime;
            }

            if (unlockedChangedPerspective && changePerspectiveEnabled && Input.GetKeyDown(KeyCode.Space))
            {
                if (cam.enabled)
                {
                    Ray ray = new Ray(transform.position + Vector3.back * 0.55f, Vector3.back);
                    RaycastHit hit;

                    if (!Physics.Raycast(ray, out hit, 60f))
                    {
                        islandManager.ChangePerspective();
                    }
                }
                else
                {
                    islandManager.ChangePerspective();
                }
            }
        }


        if (spamButton)
        {
            if (buttonToPress == "right" && Input.GetAxisRaw("Horizontal") >= 1f && pressingButton == false)
            {
                buttonPresses++;
                buttonInstructions.text = String.Format(buttonInstructionsText, buttonToPress, buttonPressesGoal - buttonPresses, textGoal);
                playerMovement.PlayerMove(1f, 0f, 50f);
                pressingButton = true;
            }

            if (buttonToPress == "space" && Input.GetKeyDown(KeyCode.Space))
            {
                buttonPresses++;
                buttonInstructions.text = String.Format(buttonInstructionsText, buttonToPress, buttonPressesGoal - buttonPresses, textGoal);
            }  
        }

        if(pressingButton && Input.GetAxisRaw("Horizontal") == 0f)
        {
            pressingButton = false;
        }

        DialogueDebug();

        // Mobile control
        /*if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    navMeshAgent.destination = hit.point;
                }
            }
        }*/
    }

    

    public void SetIInstance(IInteractable interactable)
    {
        interactableInstance = interactable;
    }

    public void ClearIInstance()
    {
        interactableInstance = null;
    }

    [YarnCommand("freeze_player")]
    public void Freeze()
    {
        freeze = !freeze;
    }

    [YarnCommand("enable_perspective")]
    public void EnablePerspective()
    {
        changePerspectiveEnabled = !changePerspectiveEnabled;
    }

    [YarnCommand("unlock_perspective")]
    public void UnlockPerspective()
    {
        unlockedChangedPerspective = !unlockedChangedPerspective;
    }

    int buttonPresses = 0;
    bool spamButton;
    bool pressingButton = false;
    string buttonToPress;
    int buttonPressesGoal;
    string textGoal;

    [YarnCommand("spam_button")]
    public IEnumerator SpamButton(string key, int buttonPressGoal, string goal)
    {
        buttonPresses = 0;
        buttonToPress = key;
        spamButton = true;
        buttonPressesGoal = buttonPressGoal;
        textGoal = goal;

        buttonInstructions.text = String.Format(buttonInstructionsText, key, buttonPressGoal, goal);
        buttonInstructionsObject.SetActive(true);
        yield return new WaitUntil(() => buttonPresses >= buttonPressGoal);
        buttonInstructionsObject.SetActive(false);
        spamButton = false;
    }

    [SerializeField] DialogueRunner dialogueRunner;
    private void DialogueDebug()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dialogueRunner.StartDialogue("IdentifyThoughts");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            dialogueRunner.StartDialogue("Relapse");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            dialogueRunner.StartDialogue("End");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dialogueRunner.StartDialogue("Start");
        }
    }
}
