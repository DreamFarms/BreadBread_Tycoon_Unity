using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> tutorialSteps;
    public TutorialUIController uiController;

    private int stepIndex;
    private int subIndex;
    private bool waiting = false;

    private void Start()
    {
        StartTutorial();
    }

    public void StartTutorial()
    {
        stepIndex = 0;
        subIndex = 0;
        ExecuteCurrentSubStep();
    }

    private void ExecuteCurrentSubStep()
    {
        var step = tutorialSteps[stepIndex];
        var sub = step.subSteps[subIndex];

        Debug.Log($"[Æ©Åä¸®¾ó] {sub.type} - {sub.description}");

        switch (sub.type)
        {
            case SubStepData.SubStepType.Dialogue:
                uiController.ShowDialogue(sub.description, ProceedNextStep);
                break;

            case SubStepData.SubStepType.ShowGuideUI:
                uiController.ShowGuide(sub.description);
                break;

            case SubStepData.SubStepType.WaitForCondition:
                uiController.ShowGuide(sub.description);
                waiting = true;
                break;
        }
    }

    IEnumerator WaitThenNext(float sec)
    {
        yield return new WaitForSeconds(sec);
        ProceedNextStep();
    }

    public void OnConditionMet(string condition)
    {
        if (!waiting) return;

        var sub = tutorialSteps[stepIndex].subSteps[subIndex];

        if (sub.type == SubStepData.SubStepType.WaitForCondition &&
            sub.conditionEventName == condition)
        {
            waiting = false;
            ProceedNextStep();
        }
    }

    void ProceedNextStep()
    {
        uiController.HideDialogue();
        uiController.HideGuide();

        subIndex++;

        if (subIndex >= tutorialSteps[stepIndex].subSteps.Count)
        {
            stepIndex++;
            subIndex = 0;
        }

        if (stepIndex < tutorialSteps.Count)
        {
            ExecuteCurrentSubStep();
        }
        else
        {
            Debug.Log("Æ©Åä¸®¾ó ¿Ï·á!");
        }
    }
}
