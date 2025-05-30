using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public List<TutorialStep> tutorialSteps;
    public TutorialUIController uiController;

    private int stepIndex;
    private int subIndex;
    private bool waiting = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitTutorial();
    }

    public void InitTutorial()
    {
        stepIndex = 0;
        subIndex = 0;
        ExecuteCurrentSubStep();
    }

    private void ExecuteCurrentSubStep()
    {
        var step = tutorialSteps[stepIndex];
        var sub = step.subSteps[subIndex];

        Debug.Log($"[Ʃ�丮��] {sub.type} - {sub.description}");

        switch (sub.type)
        {
            case SubStepData.SubStepType.Dialogue:
                uiController.ShowDialogue(sub.description, ProceedNextStep);
                break;

            case SubStepData.SubStepType.ShowGuideUI: // ���̵尡 ���� ��
                uiController.ShowGuide(sub.description);
                break;

            case SubStepData.SubStepType.WaitForCondition: // ���̵嵵 �ְ� ������ �ൿ�� ��ٷ��� �� ��
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

        // ���� ������ �̾�ų�
        if (subIndex >= tutorialSteps[stepIndex].subSteps.Count)
        {
            stepIndex++;
            subIndex = 0;
        }

        // ���� ������ ��� �����ٸ� ���� ������ �̾����.
        if (stepIndex < tutorialSteps.Count)
        {
            ExecuteCurrentSubStep();
        }
        else
        {
            Debug.Log("Ʃ�丮�� �Ϸ�!");
        }
    }
}
