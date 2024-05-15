using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] OptionView option;
    [SerializeField] TMP_StyleSheet style;
    [SerializeField] Image optionArrow;

    private bool updatedStyle;

    private float unselectedAlpha = 0.5f;

    private void FixedUpdate()
    {
        /*if(EventSystem.current.currentSelectedGameObject == base.gameObject)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        } else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, unselectedAlpha);
        }*/
        
        if (!option.interactable && !updatedStyle)
        {
            text.textStyle = style.GetStyle(500524041);
            text.color = new Color(text.color.r, text.color.g, text.color.b, unselectedAlpha);
            optionArrow.color = new Color(optionArrow.color.r, optionArrow.color.g, optionArrow.color.b, unselectedAlpha);
            updatedStyle = true;
            option.enabled = false;
        }
        if (option.interactable && updatedStyle)
        {
            text.textStyle = style.GetStyle(-1183493901);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
            optionArrow.color = new Color(optionArrow.color.r, optionArrow.color.g, optionArrow.color.b, 1f);
            updatedStyle = false;
            option.enabled = true;
        }
    }
}
