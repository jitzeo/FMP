using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] CharacterController controller;

    private bool thirdDimension = true;

    private float velocity = 5f;
    private float rotationSpeed = 720f;
    private float gravity = -9.81f;

    private void Start()
    {
        cam = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
    }

    public void PlayerMove(float x, float z)
    {
        Vector3 direction;
        if (onLadder && !exitLadder)
        {
            direction = new Vector3(0f, z, 0f) * velocity;
        }
        else if (!thirdDimension)
        {
            direction = new Vector3(x, 0f, 0f).normalized * velocity;
            direction.y = gravity;
        }
        else
        {
            direction = new Vector3(x, 0f, z).normalized * velocity;
            float angle = Vector2.SignedAngle(new Vector2(cam.transform.forward.x, cam.transform.forward.z), Vector2.up);
            direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
            direction.y = gravity;
        }
        controller.Move(direction * Time.deltaTime);

        if (direction.x != 0 || direction.z != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    [Header("Ladder movement:")]
    private Ladder activeLadder;
    public bool onLadder;
    public bool exitLadder;
    public void OnLadder(Vector3 position, Ladder currentLadder, Vector3 direction)
    {
        transform.position = position;
        activeLadder = currentLadder;
        onLadder = true;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    public void LadderExit()
    {
        controller.enabled = false;
        transform.position = activeLadder.endPos.position;
        onLadder = false;
        exitLadder = false;
        controller.enabled = true;
    }

    public void ChangePerspective()
    {
        controller.enabled = false;
        thirdDimension = !thirdDimension;

        if (thirdDimension)
        {
            Ray ray = new Ray(transform.position + new Vector3(0f, -1.5f, 1f), Vector3.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, hit.point.z + 1f);
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -20f);
        }
        controller.enabled = true;
    }

    public void MoveAgent(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            navMeshAgent.destination = hit.point;
        }
    }
}
