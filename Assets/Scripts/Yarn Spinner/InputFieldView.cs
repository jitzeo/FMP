using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;
using Unity.VisualScripting;

public class InputFieldView : MonoBehaviour
{
    [SerializeField] GameObject inputView;

    [SerializeField] TMP_InputField input;

    [SerializeField] TMP_Text text;
    
    static string inputText = "NaN";

    public bool inputComplete;

    public void OnInputSubmit()
    {
        Debug.Log("Submit input");
        if(Input.GetButton("Submit"))
        {
            inputComplete = true;
            inputText = input.text;
        }       
    }

    [YarnCommand("input_field")]
    public IEnumerator InputView(string lastLine)
    {
        text.text = lastLine;
        inputView.SetActive(true);
        input.Select();
        yield return new WaitUntil(() => inputComplete);
        input.text = "";
        inputView.SetActive(false);
        inputComplete = false;
    }

    [YarnFunction("getInputText")]
    public static string GetInputText()
    {
        return inputText;
    }
}
