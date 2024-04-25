using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] CharacterController controller;

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
        Vector3 direction = new Vector3(x, 0f, z).normalized * velocity;
        direction = Quaternion.AngleAxis(45f, Vector3.up) * direction;
        direction.y = gravity;
        controller.Move(direction * Time.deltaTime);

        if (direction.x != 0 || direction.z != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
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
