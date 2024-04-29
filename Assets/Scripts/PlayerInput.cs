using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    private float x;
    private float z;

    [SerializeField] Camera cam;
    [SerializeField] GameObject center;
    private float rotateSpeed = 50f;
    private float zoomSpeed = 50f;

    [SerializeField] Camera cam2D;

    private IInteractable interactableInstance;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cam = Camera.main;
        cam2D = GameObject.Find("2D Camera").GetComponent<Camera>();
        center = GameObject.Find("Center");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //playerMovement.MoveAgent(Input.mousePosition);   
        }

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        playerMovement.PlayerMove(x, z);

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cam.enabled)
            {
                Ray ray = new Ray(transform.position + Vector3.back * 0.55f, Vector3.back);
                RaycastHit hit;

                if (!Physics.Raycast(ray, out hit, 60f))
                {
                    ChangePerspective();
                }
            } else
            {
                ChangePerspective();
            }
        }

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

    private void ChangePerspective()
    {
        cam.enabled = !cam.enabled;
        cam2D.enabled = !cam2D.enabled;

        playerMovement.ChangePerspective();
    }

    public void SetIInstance(IInteractable interactable)
    {
        interactableInstance = interactable;
    }

    public void ClearIInstance()
    {
        interactableInstance = null;
    }
}
