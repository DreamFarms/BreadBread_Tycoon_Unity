using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image questIconImage;
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private Slider questProgressSlider;
    [SerializeField] private TMP_Text questProgressText;

    private Quest linkedQuest;

    public void Setup(Quest quest, Sprite backgroundSprite, Sprite questIconSprite)
    {
        linkedQuest = quest;

        backgroundImage.sprite = backgroundSprite;
        questIconImage.sprite = questIconSprite;
        questNameText.text = quest.QuestName;

        UpdateProgress();
    }

    public void UpdateProgress()
    {
        if (linkedQuest == null) return;

        throw new NotImplementedException();
    }
}
