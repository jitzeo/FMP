using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using System;
using Yarn;
using Yarn.Unity;

public class IslandManager : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Camera cam;

    [SerializeField] Camera camMain;
    [SerializeField] Camera cam2D;

    [SerializeField] GameObject mainIsland;
    [SerializeField] GameObject island2D;
    [SerializeField] GameObject startIsland;

    [SerializeField] Transform startPosDino;

    private Dictionary<string, GameObject> islands = new Dictionary<string, GameObject>();

    private void Start()
    {
        islands.Add("main", mainIsland);
        islands.Add("2D", island2D);
        islands.Add("start", startIsland);
    }

    public void EnableIsland(string id, bool enable)
    {
        islands[id].SetActive(enable);
        if(id == "2D")
        {
            cam2D.enabled = enable;
            playerMovement.thirdDimension = !enable;
        }
        else
        {
            camMain.enabled = enable;
            playerMovement.thirdDimension = enable;
        }
    }

    [YarnCommand("change_perspective")]
    public void ChangePerspective()
    {
        ChangeCamera();
        ChangeIsland();
    }

    [YarnCommand("change_start")]
    public void ChangeStart()
    {
        EnableIsland("2D", true);
        playerMovement.Teleport(startPosDino.position);
        EnableIsland("start", false);
    }
    
    [YarnCommand("change_camera")]
    public void ChangeCamera()
    {
        camMain.enabled = !camMain.enabled;
        cam2D.enabled = !cam2D.enabled;
    }

    [YarnCommand("change_island")]
    public void ChangeIsland() 
    {
        foreach (GameObject island in islands.Values)
        {
            if (island != startIsland)
            {
                island.SetActive(!island.activeSelf);
            }
        }

        playerMovement.ChangePerspective();
    }
}
