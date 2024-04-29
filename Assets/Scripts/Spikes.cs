using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour, ISwitchable
{
    [SerializeField] bool isActive;
    public bool IsActive => isActive;

    public void Activate()
    {
        isActive = true;
        Debug.Log("Activate");
    }

    public void Deactivate()
    {
        isActive = false;
        Debug.Log("Deactivate");
    }
}
