using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] Transform startPos;
    [SerializeField] public Transform endPos;

    private Vector3 direction;

    private void Start()
    {
        direction = endPos.position - startPos.position;
        direction.y = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && !playerMovement.onLadder)
            {
                playerMovement.OnLadder(startPos.position, this, direction);
            }
        }
    }
}
