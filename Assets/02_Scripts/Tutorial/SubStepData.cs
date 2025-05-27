using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SubStepData
{
    public enum SubStepType { Dialogue, WaitForCondition } // dialogue : ��� ���� | waitforcondition : �ൿ�� ��ٷ��� �Ѿ

    public SubStepType type;

    [TextArea]
    public string description; // ���

    public string conditionEventName; // ���� �̸�

    public UnityEvent onStart;
    public UnityEvent onComplete;
}
