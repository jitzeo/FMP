using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] Transform startPosBot;
    [SerializeField] Transform startPosTop;
    [SerializeField] Transform checkStartPos;
 
    private Vector3 direction;

    private void Start()
    {
        direction = transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && !playerMovement.onLadder)
            {
                if(other.transform.position.y > checkStartPos.position.y) 
                {
                    playerMovement.OnLadder(startPosTop.position, direction);
                }
                else
                {
                    playerMovement.OnLadder(startPosBot.position, direction);
                }     
            }
        }
    }
}
