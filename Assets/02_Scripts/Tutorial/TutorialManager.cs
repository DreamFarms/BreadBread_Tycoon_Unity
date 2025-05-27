using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> tutorialSteps;
    private int currentStepIndex = 0;
    private int currentSubStepIndex = 0;

    private TutorialStep CurrentStep => tutorialSteps[currentStepIndex];
    private SubStepData CurrentSubStep => CurrentStep.subSteps[currentSubStepIndex];

    private bool waitingForCondition = false;

    private void Start()
    {
        StartCoroutine(PlayCurrentSubStep());
    }

    IEnumerator PlayCurrentSubStep()
    {
        var step = CurrentSubStep;

        step.onStart?.Invoke();

        if (step.type == SubStepData.SubStepType.Dialogue) // ��ȭ�� �����̸�
        {
            // ��: ��� ��� �� 2�� ���
            ShowDialogue(step.description);
            yield return new WaitForSeconds(2f); // ���߿� �ؽ�Ʈ �� ���� �� �Ѿ�� ���� ���� ����
            step.onComplete?.Invoke();
            GoToNextSubStep();
        }
        else if (step.type == SubStepData.SubStepType.WaitForCondition)
        {
            ShowUI(step.description); // ��: "������ Ŭ���ϼ���"
            waitingForCondition = true;
        }
    }

    public void OnConditionMet(string condition)
    {
        if (!waitingForCondition) return;

        var step = CurrentSubStep;

        if (step.type == SubStepData.SubStepType.WaitForCondition &&
            step.conditionEventName == condition)
        {
            step.onComplete?.Invoke();
            waitingForCondition = false;
            GoToNextSubStep();
        }
    }

    void GoToNextSubStep()
    {
        currentSubStepIndex++;
        if (currentSubStepIndex >= CurrentStep.subSteps.Count)
        {
            currentStepIndex++;
            currentSubStepIndex = 0;

            if (currentStepIndex >= tutorialSteps.Count)
            {
                Debug.Log("Ʃ�丮�� �Ϸ�!");
                return;
            }
        }

        StartCoroutine(PlayCurrentSubStep());
    }

    void ShowDialogue(string text)
    {
        Debug.Log("[���] " + text);
    }

    void ShowUI(string guideText)
    {
        Debug.Log("[�ȳ� UI] " + guideText);
    }
}
