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

        if (step.type == SubStepData.SubStepType.Dialogue) // 대화형 스텝이면
        {
            // 예: 대사 출력 후 2초 대기
            ShowDialogue(step.description);
            yield return new WaitForSeconds(2f); // 나중엔 텍스트 다 읽은 뒤 넘어가게 만들 수도 있음
            step.onComplete?.Invoke();
            GoToNextSubStep();
        }
        else if (step.type == SubStepData.SubStepType.WaitForCondition)
        {
            ShowUI(step.description); // 예: "매장을 클릭하세요"
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
                Debug.Log("튜토리얼 완료!");
                return;
            }
        }

        StartCoroutine(PlayCurrentSubStep());
    }

    void ShowDialogue(string text)
    {
        Debug.Log("[대사] " + text);
    }

    void ShowUI(string guideText)
    {
        Debug.Log("[안내 UI] " + guideText);
    }
}
