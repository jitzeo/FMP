using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    PlayerMovement playerMovement;
    private float x;
    private float z;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
}
