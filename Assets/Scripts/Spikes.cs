using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour, ISwitchable
{
    [SerializeField] Vector3 deltaPosition;
    private Vector3 startPosition;
    private float moveSpeed = 3f;

    [SerializeField] Collider col;

    [SerializeField] bool isActive;
    public bool IsActive => isActive;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    private void Update()
    {
        if (isActive)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * moveSpeed);
            col.enabled = true;
        } else
        {
            transform.position = Vector3.Lerp(transform.position, startPosition + deltaPosition, Time.deltaTime * moveSpeed);
            col.enabled = false;
        }
    }
}
