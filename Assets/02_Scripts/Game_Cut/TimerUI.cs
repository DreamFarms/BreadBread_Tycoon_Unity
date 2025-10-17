using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Slider timeSlider;
    public float duration;
    
    private Coroutine timerCoroutine;

    public event Action OnTimerFinished;

    public void StartTimer(float customDuration)
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        duration = customDuration;
        timeSlider.maxValue = duration;
        timeSlider.value = duration;

        timerCoroutine = StartCoroutine(RunTimer());
    }

    public void ResetTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        timeSlider.value = duration;
    }

    private IEnumerator RunTimer()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            timeSlider.value = duration - elapsed;
            yield return null;
        }

        OnTimerFinished?.Invoke();
        timeSlider.value = 0;
    }
}
