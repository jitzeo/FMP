using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    private float velocity = 1f;
    [SerializeField] private LineRenderer lineRenderer;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
    }

    public void Move(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            navMeshAgent.destination = hit.point;
            lineRenderer.SetPositions(new Vector3[] { ray.origin, hit.point });
            Debug.Log(hit.point);
        }
    }
}
