using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SubStepData
{
    public enum SubStepType { Dialogue, WaitForCondition } // dialogue : 대사 진행 | waitforcondition : 행동을 기다려야 넘어감

    public SubStepType type;

    [TextArea]
    public string description; // 대사

    public string conditionEventName; // 조건 이름

    public UnityEvent onStart;
    public UnityEvent onComplete;
}
