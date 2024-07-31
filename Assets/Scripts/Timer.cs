using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Slider timeoutSlider;

    [SerializeField]
    private float timeLimit = 60f;

    private float currentTime;

    [SerializeField]
    private TextMeshProUGUI timeoutText;

    private void Start()
    {
        currentTime = timeLimit;
        SetCurrentTimeText();
        StartCoroutine(nameof(CountDownTimerRoutine));
    }

    IEnumerator CountDownTimerRoutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 다른 성능의 pc에서도 동일한 시간이 빠지게 됨
            timeoutSlider.value = currentTime / timeLimit;
            SetCurrentTimeText();
            yield return null;
        }

        BerryPickerManager.Instance.GameOver();
    }

    private void SetCurrentTimeText()
    {
        int timeSec = Mathf.CeilToInt(currentTime); // 올림
        timeoutText.SetText(timeSec.ToString());
    }
}
