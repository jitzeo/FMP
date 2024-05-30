using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Yarn.Unity;

public class BOFS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadCondition());
    }

    IEnumerator LoadCondition() 
    {
        string url = "/fetch_condition";

        using (UnityWebRequest request = UnityWebRequest.Get(url)) 
        {

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError) 
            {
                Debug.Log(request.error);
            }
            else 
            {
                string response = request.downloadHandler.text;

                if (response.Length == 0) 
                {
                    Debug.Log("Unable to load condition! Does the participant have a valid session?");
                }
                else 
                {
                    Debug.Log(response);
                }
            }
        }
    }

    [YarnCommand("post_answer")]
    public void PostAnswer(string preFearLevel, string postFearLevel) {
        WWWForm frm = new WWWForm();
        frm.AddField("preFearLevel", preFearLevel);
        frm.AddField("postFearLevel", postFearLevel);

        try
        {
            var request = UnityWebRequest.Post("#", frm);
            request.SendWebRequest();
            Debug.Log(request.result);
            Debug.Log("Data send");
            StartCoroutine(WaitForDispose(request));
        }
        catch (Exception ex)
        {
            Debug.Log("Error in PostInput(): " + ex.Message);
        }
    }

    private IEnumerator WaitForDispose(UnityWebRequest request)
    {
        yield return new WaitUntil(() => request.isDone);
        {
            request.Dispose();
        }
    }

    [DllImport("__Internal")]
    public static extern void RedirectBOF();

    // Can't add RedirectBOF() to onClick directly.
    [YarnCommand("redirect_bof")]
    public void RedirectBOFClicked() {
        RedirectBOF();
    }

    public void RedirectBOFDeprecated() {
        // Deprecated way of redirecting participants to the next page in the experiment
        Application.ExternalEval("window.location.href = \"/redirect_next_page\";"); 

        // Make the game simulation stop.
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
