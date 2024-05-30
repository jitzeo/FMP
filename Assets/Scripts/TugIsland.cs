using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class TugIsland : MonoBehaviour
{
    [SerializeField] IslandManager islandManager;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Fade fade;
    [SerializeField] DialogueRunner dialogueRunner;

    [SerializeField] Transform rotateAxis;
    [SerializeField] Transform player;
    private Vector3 startPlayerPos;
    private Quaternion startPlayerRotation;


    [SerializeField] GameObject tagInstructions;
    [SerializeField] TMP_Text tagInstructionsText;
    private string instructionString = "Click and quickly drag the island to the right {0} times to change perspective.";

    private float mousePos;
    private float mouseDelta;
    private bool tugAvailable = true;
    private bool tugActive = false;
    private bool rotateIsland;
    private bool resetRotationIsland;
    private float rotationDelta;
    private float currentRotation;
    private int tugCount;

    private float minimalTug = 20f;

    [SerializeField] float turnTime = 0.5f;
    [SerializeField] float tugFactor = 0.05f;
    float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        mousePos = Input.mousePosition.x;
        tagInstructionsText.text = string.Format(instructionString, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (tugCount < 3 && tugActive)
        {
            TugInteraction();
        }
        else if (tugCount >= 3 && tugActive)
        {
            transform.RotateAround(rotateAxis.position, Vector3.up, Mathf.Lerp(rotationDelta, -15f, t));
            player.RotateAround(rotateAxis.position, Vector3.up, Mathf.Lerp(rotationDelta, -15f, t));
            t += 0.5f * Time.deltaTime;
        }
    }

    private void TugInteraction()
    {
        currentRotation = transform.rotation.eulerAngles.y;

        if (Input.GetMouseButton(0) && tugAvailable)
        {
            mouseDelta = Input.mousePosition.x - mousePos;
            rotationDelta = -mouseDelta * tugFactor;

            if (currentRotation < 50 || currentRotation > 290 || (currentRotation < 75 && rotationDelta < 0) || (currentRotation > 265 && rotationDelta > 0))
            {
                transform.RotateAround(rotateAxis.position, Vector3.up, rotationDelta);
                player.RotateAround(rotateAxis.position, Vector3.up, rotationDelta);
            }
        }

        if (mouseDelta > minimalTug && tugAvailable)
        {
            //Rotate the island a bit before turning back
            tugAvailable = false;
            mouseDelta = 0;
            StartCoroutine(RotateIslandAfterRelease());

            tugCount++;
            tagInstructionsText.text = string.Format(instructionString, 3 - tugCount);

            if (tugCount >= 3)
            {
                StartCoroutine(TransformAnimation());
                return;
            }
        }
        mousePos = Input.mousePosition.x;

        if (rotateIsland && (currentRotation < 50 || currentRotation > 290))
        {
            transform.RotateAround(rotateAxis.position, Vector3.up, rotationDelta);
            player.RotateAround(rotateAxis.position, Vector3.up, rotationDelta);
        }

        if (resetRotationIsland && (currentRotation >= 5 || currentRotation <= 355))
        {
            if (currentRotation < 75)
            {
                transform.RotateAround(rotateAxis.position, Vector3.up, -currentRotation / turnTime * Time.deltaTime);
                player.RotateAround(rotateAxis.position, Vector3.up, -currentRotation / turnTime * Time.deltaTime);
            }
            else if (currentRotation > 265)
            {
                transform.RotateAround(rotateAxis.position, Vector3.up, (360 - currentRotation) / turnTime * Time.deltaTime);
                player.RotateAround(rotateAxis.position, Vector3.up, (360 - currentRotation) / turnTime * Time.deltaTime);
            }
        }
    }

    private IEnumerator RotateIslandAfterRelease()
    {
        rotateIsland = true;
        yield return new WaitForSecondsRealtime(0.2f);
        rotateIsland = false;
        //Start coroutine to wait until Island is back in original position
        yield return ResetRotationIsland();
    }

    private IEnumerator ResetRotationIsland()
    {        
        resetRotationIsland = true;
        yield return new WaitUntil(() => (currentRotation <= 5 || currentRotation >= 355));
        resetRotationIsland = false;
        tugAvailable = true;
    }

    private IEnumerator TransformAnimation()
    {
        tagInstructions.SetActive(false);
        yield return new WaitForSecondsRealtime(2f);
        tugActive = false;
        yield return fade.FadeTransition("FadeInLeft");

        // Reset the islands position and rotation to normal
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        playerMovement.Teleport(startPlayerPos);
        player.rotation = startPlayerRotation;
        yield return new WaitForSecondsRealtime(0.3f);
        
        StartCoroutine(fade.FadeTransition("FadeOutLeft"));
        dialogueRunner.StartDialogue("Investigate2");
        islandManager.ChangePerspective();
        this.enabled = false;
    }


    [YarnCommand("tug_interaction")]
    public void ActivateTugInteraction(bool activate)
    {
        if (activate)
        {
            startPlayerPos = player.position;
            startPlayerRotation = player.rotation;
        }
        tugActive = activate;
        tagInstructions.SetActive(activate);
    }
}
