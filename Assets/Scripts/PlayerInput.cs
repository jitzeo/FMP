using System;
using System.Collections;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Yarn.Unity;

public class PlayerInput : MonoBehaviour
{
    [Header("Player movement")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] IslandManager islandManager;
    [SerializeField] CharacterController controller;
    private float x;
    private float z;

    [Header("Camera Control")]
    [SerializeField] Camera cam;
    [SerializeField] Camera cam2D;
    [SerializeField] CustomCameraController camController;
    //[SerializeField] GameObject center;
    private float rotateSpeed = 50f;
    private float zoomSpeed = 50f;

    [Header("Dialogue Control")]
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] LineView lineView;

    [Header("Spam Button Interaction")]
    [SerializeField] GameObject buttonInstructionsObject;
    [SerializeField] TMP_Text buttonInstructions;
    private string buttonInstructionsText;

    [Header("Player control features")]
    [SerializeField] bool unlockedChangedPerspective;
    [SerializeField] bool changePerspectiveEnabled;
    public bool freezePlayer;

    private IInteractable interactableInstance;

    private bool focus;

    private void Start()
    {
        buttonInstructionsText = "Press {0} {1} times to {2}";
    }

    void Update()
    {
        // Code to make sure WebGL does not captue all keyboard input when not focused.
        #if !UNITY_EDITOR && UNITY_WEBGL
            if (!Application.isFocused && !focus)
            {
                WebGLInput.captureAllKeyboardInput = false;
                focus = true;
            }
            if (Application.isFocused && focus)
            {
                WebGLInput.captureAllKeyboardInput = true;
                focus = false;
            }
        #endif

        
        if (!freezePlayer)
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
            playerMovement.PlayerMove(x, z);
        

            if (Input.GetKey(KeyCode.E))
            {
                camController.RotateCamera(-rotateSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                camController.RotateCamera(rotateSpeed * Time.deltaTime);
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

            if (unlockedChangedPerspective && changePerspectiveEnabled && controller.isGrounded && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
            {
                if (cam.enabled)
                {
                    Ray ray = new Ray(transform.position + Vector3.back * 0.55f, Vector3.back);
                    RaycastHit hit;
                    Ray ray2 = new Ray(transform.position + Vector3.forward * 0.55f, Vector3.forward);
                    RaycastHit hit2;

                    if (!Physics.Raycast(ray, out hit, 60f) && !Physics.Raycast(ray2, out hit2, 60f))
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
        else
        {
            playerMovement.PlayerMove(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Return) && dialogueRunner.IsDialogueRunning)
        {
            lineView.OnContinueClicked();
        }

        if (spamButton)
        {
            if (buttonToPress == "right" && Input.GetAxisRaw("Horizontal") >= 1f && pressingButton == false)
            {
                buttonPresses++;
                buttonInstructions.text = String.Format(buttonInstructionsText, buttonToPress, buttonPressesGoal - buttonPresses, textGoal);
                playerMovement.PlayerMove(1f, 0f, 9f); //Time.deltaTime on WebGL is a factor 10 smaller then in the editor.
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
    public void FreezePlayer()
    {
        freezePlayer = !freezePlayer;
        if (freezePlayer)
        {
            playerMovement.PlayerMove(0f, 0f); // Turns velocity of character controller to 0 to stop walking animation
        }
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
        playerMovement.PlayerMove(0f, 0f); // Turns velocity of character controller to 0 to stop walking animation
    }
}
