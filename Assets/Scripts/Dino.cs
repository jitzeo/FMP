using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class Dino : MonoBehaviour
{
    [SerializeField] IslandManager islandManager;
    
    [SerializeField] Vector3 startPos;
    float velocity = 5.7f;
    private bool freeze = true;

    [SerializeField] GameObject dinoPassive;

    [SerializeField] Transform playerSpawn;

    private void Start()
    {
        startPos = transform.position;
        playerSpawn = GameObject.Find("DinoPlayerSpawn").GetComponent<Transform>();
        //dinoPassive = GameObject.Find("DinoPassive");
    }

    private void FixedUpdate()
    {
        if (!freeze)
        {
            transform.position += Vector3.left * velocity * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ResetAct(other);
        } else
        {
            StartCoroutine(FallDown());
        }
    }

    private void ResetAct(Collider player)
    {
        transform.position = startPos;
        player.enabled = false;
        player.transform.position = playerSpawn.position;
        player.enabled = true;
    }

    private IEnumerator FallDown()
    {
        freeze = true;
        yield return new WaitForSecondsRealtime(2f);
        dinoPassive.SetActive(true);
        gameObject.SetActive(false);
    }

    [YarnCommand("unfreeze_dino")]
    public void Unfreeze()
    {
        freeze = false;
    }
}
