using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] CharacterController controller;

    public bool thirdDimension = true;
    
    private bool moveToPositionBool;
    private Vector3 moveToDirection;
    private Vector3 moveToPosition;

    private bool moveInDirectionBool;

    private float velocity = 5f;
    private float rotationSpeed = 720f;
    private float gravity = -9.81f;

    private bool turn;

    [Header("Stamina")]
    [SerializeField] Stamina stamina;
    public bool staminaActive;

    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (moveToPositionBool && moveToPosition != null)
        {
            PlayerMove(moveToDirection.x, moveToDirection.z, 5f, moveToPosition);
        }

        if (moveInDirectionBool && moveToDirection != null)
        {
            PlayerMove(moveToDirection.x, moveToDirection.z);
        }

        if (turn && moveToDirection != null)
        {
            LookDirection(moveToDirection);
        }
    }

    public void PlayerMove(float x, float z, float v = 0f, Vector3? position = null)
    {
        if(v != 0f)
        {
            velocity = v;
        }
        else
        {
            velocity = 5f;
        }
        
        Vector3 direction = Vector3.zero;
        if (onLadder && !exitLadder)
        {
            direction = new Vector3(0f, z, 0f) * velocity;
        }
        else if (!thirdDimension && (x != 0 || z != 0))
        {
            direction = new Vector3(x, 0f, 0f).normalized * velocity;
            direction.y = gravity;
        }
        else if ((x != 0 || z != 0))
        {
            direction = new Vector3(x, 0f, z).normalized * velocity;
            float angle = Vector2.SignedAngle(new Vector2(cam.transform.forward.x, cam.transform.forward.z), Vector2.up);
            direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
            direction.y = gravity;
        }

        if (staminaActive)
        {
            float staminaFactor = stamina.UpdateStamina(x, z);
            direction = direction * staminaFactor;
        }

        if (controller.enabled)
        {
            controller.Move(direction * Time.deltaTime);
            //Debug.Log("Movement vector: " + direction * Time.deltaTime + " direction: " +direction + "deltaTime: " + Time.deltaTime);
        }

        if (direction != Vector3.zero && !onLadder)
        {
            LookDirection(direction);
        } 
    }

    [YarnCommand("move_player")]
    public IEnumerator MoveInDirection(string direction, float wait = 0)
    {
        if(direction == "left")
        {
            moveToDirection = Vector3.left;
            moveInDirectionBool = true;
        } 
        else if (direction == "right")
        {
            moveToDirection = Vector3.right;
            moveInDirectionBool = true;
        } 
        else if (direction == "stop") {
            moveInDirectionBool = false;
        }

        yield return new WaitForSecondsRealtime(wait);
        if(wait > 0)
        {
            moveInDirectionBool = false;
        }
    }

    [YarnCommand("turn_player")]
    public IEnumerator TurnPlayer(string direction)
    {
        if(direction == "left")
        {
            moveToDirection = Vector3.left;
        } 
        else if(direction == "right")
        {
            moveToDirection = Vector3.right;
        }
        turn = true;
        yield return new WaitUntil(() => transform.forward == moveToDirection);
        turn = false;
    }

    
    public IEnumerator MoveToPosition(Vector3 position)
    {
        moveToDirection = position - transform.position;
        moveToPosition = position;
        moveToPositionBool = true;
        yield return new WaitUntil(() => (position - transform.position).magnitude <= 0.2f);
        moveToPositionBool = false;
    }

    [Header("Ladder movement:")]
    public bool onLadder;
    public bool exitLadder;
    private Ladder activeLadder;
    
    public void OnLadder(Vector3 startPos, Ladder currentLadder, Vector3 direction)
    {
        Teleport(startPos);
        activeLadder = currentLadder;
        onLadder = true;
        RotatePlayer(direction);
    }

    public void LookDirection(Vector3 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    public void LadderExit(Vector3 endPos)
    {
        Teleport(endPos);
        onLadder = false;
        exitLadder = false;
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

    public void Teleport(Vector3 position)
    {
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }

    [YarnCommand("rotate_player")]
    public void RotatePlayer(Vector3 direction = new Vector3())
    {
        if(direction == Vector3.zero)
        {
            direction = Vector3.right;
        }
        transform.rotation = Quaternion.LookRotation(direction);
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
