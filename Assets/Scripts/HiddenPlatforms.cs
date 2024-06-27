using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPlatforms : MonoBehaviour, ISwitchable
{
    [SerializeField] Vector3 endPosition;
    private float moveSpeed = 2f;

    private bool isActive;
    public bool IsActive => isActive;

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
            transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * moveSpeed);
        }
    }
}
