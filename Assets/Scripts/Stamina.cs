using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class Stamina : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Camera cam;
    [SerializeField] GameObject staminaCanvas;
    [SerializeField] Slider staminaBar;
    [SerializeField] Animation refillStamina;

    [SerializeField] Animator animatorPlayer;

    private float staminaMax = 10;
    private float staminaAmount = 10;
    private float staminaDecreasePerSecond = 10;
    private float staminaIncreasePerSecond = 10;

    private float normalSpeedFactor = 0.8f;
    private float decreaseSpeedFactor = 0.15f;

    private bool staminaDepleted;
    private bool animInterupted;

    [YarnCommand("activate_stamina")]
    public void ActivateStamina(bool activate)
    {
        playerMovement.staminaActive = activate;
        staminaCanvas.SetActive(activate);
    }

    private void Update()
    {
        if(staminaCanvas.activeSelf && !staminaDepleted)
        {
            staminaBar.value = staminaAmount/staminaMax;
        }

        staminaCanvas.transform.rotation = cam.transform.rotation; 
    }

    public float UpdateStamina(float x, float z)
    {
        if(staminaAmount > 0 && !staminaDepleted && (x != 0 || z != 0))
        {
            DecreaseStamina();
            return normalSpeedFactor;
        } 
        else if (x == 0 && z == 0 && staminaAmount > 0 && staminaAmount < staminaMax && !staminaDepleted)
        {
            IncreaseStamina();
            return normalSpeedFactor;
        }
        else if (!staminaDepleted && staminaAmount <= 0)
        {
            staminaDepleted = true;
            StartCoroutine(ResetStamina());
            return decreaseSpeedFactor;
        }
        else if (staminaDepleted)
        {
            IncreaseStamina();
            return decreaseSpeedFactor;
        }
        else
        {
            return normalSpeedFactor;
        }
        
    }

    void IncreaseStamina()
    {
        staminaAmount += staminaIncreasePerSecond * Time.deltaTime;
    }

    void DecreaseStamina()
    {
        staminaAmount -= staminaDecreasePerSecond * Time.deltaTime;
    }

    private IEnumerator ResetStamina()
    {
        refillStamina.Play();
        animatorPlayer.speed = 0.5f;
        yield return new WaitUntil(() => !refillStamina.isPlaying);
        if (!animInterupted)
        {
            staminaAmount = staminaMax;
        }
        staminaDepleted = false;
        animatorPlayer.speed = 1f;
        animInterupted = false;
    }

    [YarnCommand("deplete_stamina")]
    public void DepleteStamina()
    {
        if (refillStamina.isPlaying)
        {
            animInterupted = true;
            refillStamina.Stop();
        }

        staminaAmount = 1;
    }
}
