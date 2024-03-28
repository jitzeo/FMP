using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerAudio), typeof(PlayerInput), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerAudio playerAudio;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
    }
}
public class PlayerAudio : MonoBehaviour
{
    
}
public class PlayerInput : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    private Touch touch;
    private float velocity = 1f;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Update(){
        if(Input.touchCount > 0)
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
        }
    }

}
public class PlayerMovement : MonoBehaviour
{
    
}
