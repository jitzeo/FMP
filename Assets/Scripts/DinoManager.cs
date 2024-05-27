using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DinoManager : MonoBehaviour
{
    [SerializeField] GameObject dinoActive;
    [SerializeField] GameObject dinoPassive;
    [SerializeField] GameObject dinoEnd;

    [SerializeField] Animation anim;

    private static Dictionary<string, GameObject> dinos = new Dictionary<string, GameObject>();

    private void Start()
    {
        dinos.Add("active", dinoActive);
        dinos.Add("passive", dinoPassive);
        dinos.Add("end", dinoEnd);
    }

    [YarnCommand("activate_dino")]
    public static void ActivateDino(string dinoID)
    {
        foreach(string dino in dinos.Keys)
        {
            if (dinoID == dino)
            {
                dinos[dinoID].SetActive(true);
            } else if (dinos[dino].activeSelf)
            {
                dinos[dino].SetActive(false);
            }
        }
    }
}
