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

    private float staminaMax = 10;
    private float staminaAmount = 10;
    private float staminaDecreasePerSecond = 8;
    private float staminaIncreasePerSecond = 10;

    private float normalSpeedFactor = 0.9f;
    private float decreaseSpeedFactor = 0.1f;

    private bool staminaDepleted;

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
        yield return new WaitUntil(() => !refillStamina.isPlaying);
        staminaAmount = staminaMax;
        staminaDepleted = false;
    }
}
