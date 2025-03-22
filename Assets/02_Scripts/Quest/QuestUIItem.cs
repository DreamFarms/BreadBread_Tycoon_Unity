using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIItem : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;           // ����Ʈ ����
    [SerializeField] private TMP_Text descriptionText;     // ����Ʈ ����
    [SerializeField] private TMP_Text progressText;        // ����Ʈ ���൵
    [SerializeField] private Button completeButton;    // �Ϸ� ��ư

    private Quest _quest;

    // ����Ʈ UI �ʱ�ȭ
    public void Initialize(Quest quest)
    {
        _quest = quest;

        // titleText.text = quest.Title;
        descriptionText.text = quest.Description;
        progressText.text = quest.GetProgressText();

        // �Ϸ� ��ư Ŭ�� �̺�Ʈ ����
        completeButton.onClick.AddListener(OnCompleteButtonClicked);

        // �ʱ� ���� ������Ʈ
        UpdateStatus();
    }

    // ����Ʈ ���� ������Ʈ
    public void UpdateStatus()
    {
        // ����Ʈ�� �Ϸ�Ǿ��� ���� ��ư Ȱ��ȭ
        completeButton.interactable = (_quest.Status == QuestStatus.Completed);
    }

    // ����Ʈ ���൵ ������Ʈ
    public void UpdateProgress()
    {
        progressText.text = _quest.GetProgressText();
        UpdateStatus();
    }

    // �Ϸ� ��ư Ŭ�� �̺�Ʈ ó��
    private void OnCompleteButtonClicked()
    {
        QuestManager.Instance.CompleteQuest(_quest.QuestId);
    }

    // ������Ʈ ���� �� �̺�Ʈ ������ ����
    private void OnDestroy()
    {
        completeButton.onClick.RemoveListener(OnCompleteButtonClicked);
    }
}
