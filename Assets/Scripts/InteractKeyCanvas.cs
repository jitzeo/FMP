using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractKeyCanvas : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.rotation = cam.transform.rotation;
    }
}
