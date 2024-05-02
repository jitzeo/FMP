using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using System;

public class IslandManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Camera cam;

    [SerializeField] Camera camMain;
    [SerializeField] Camera cam2D;

    [SerializeField] GameObject mainIsland;
    [SerializeField] GameObject island2D;

    private Dictionary<string, GameObject> islands = new Dictionary<string, GameObject>();

    private void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        //mainIsland = GameObject.Find("Main Island");
        //island2D = GameObject.Find("2D Island");
        islands.Add("main", mainIsland);
        islands.Add("2D", island2D);

        camMain = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam2D = GameObject.Find("2D Camera").GetComponent<Camera>();
    }

    public void ChangePerspective()
    {
        camMain.enabled = !camMain.enabled;
        cam2D.enabled = !cam2D.enabled;
        foreach (GameObject island in islands.Values)
        {
            island.SetActive(!island.activeSelf);
        }

        playerMovement.ChangePerspective();
    }
}
