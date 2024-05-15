using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject[] clientObjects;
    private List<ISwitchable> clients = new List<ISwitchable>();

    [SerializeField] GameObject canvas;

    private void Start()
    {
        foreach(GameObject clientObject in clientObjects)
        {
            clients.Add(clientObject.GetComponent<ISwitchable>());
        }
    }
    public void InteractLogic()
    {
        foreach (ISwitchable client in clients)
        {
            if (client.IsActive)
            {
                client.Deactivate();
            }
            else
            {
                client.Activate();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInput>().SetIInstance(this);
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInput>().ClearIInstance();
            canvas.SetActive(false);
        }
    }
}
