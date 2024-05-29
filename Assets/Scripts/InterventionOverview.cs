using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class InterventionOverview : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] GameObject overview;
    [SerializeField] List<TMP_Text> questions;

    private int questionIndex;

    void Start()
    {
        dialogueRunner.AddCommandHandler("show_overview", ActivateOverview);
    }

    public void AddAnswer(string ans)
    {
        questions[questionIndex].text = string.Format(questions[questionIndex].text, ans);
        questionIndex++;
    }

    public void ActivateOverview()
    {
        overview.SetActive(true);
    }

    public void DeactivateOverview()
    {
        overview.SetActive(false);
    }
}
