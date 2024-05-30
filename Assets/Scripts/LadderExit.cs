using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderExit : MonoBehaviour
{
    [SerializeField] Transform endPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement.onLadder)
            {
                Debug.Log("Exit Ladder");
                playerMovement.LadderExit(endPos.position);
            }
        }
    }
}
