using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    [SerializeField] Image checkmark;
    [SerializeField] Sprite completed;
    [SerializeField] Sprite notCompleted;
    [SerializeField] ObjectiveFunction objectiveMethod;

    public void Update()
    {
        if (FindObjectOfType<ScenarioObjectives>().CheckObjective(objectiveMethod))
        {
            checkmark.sprite = completed;
        }
        else
        {
            checkmark.sprite = notCompleted;
        }
    }
}
