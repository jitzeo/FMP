using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBoundary : MonoBehaviour, ISwitchable
{
    [SerializeField] Collider col;
    
    private bool isActive = true;
    public bool IsActive => isActive;

    public void Activate()
    {
        //Only deactivates when platforms have risen
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
        col.enabled = false;
    }
}
