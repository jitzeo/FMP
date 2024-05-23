using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    private Vector3 distanceVector;

    void Start()
    {
        distanceVector = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + distanceVector;
    }
}
