using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject topUIGroup;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text timerText;
    private float initTimer;

    private void Start()
    {
        topUIGroup.SetActive(true);
        nicknameText.text = InfoManager.Instance.nickName;
        InfoManager.Instance.coin = 100;
        InfoManager.Instance.cash = 0;
        coinText.text = InfoManager.Instance.coin.ToString() + " ¿ø";
        cashText.text = InfoManager.Instance.cash.ToString() + " ¿ø";
        initTimer = InfoManager.Instance.timer;
        timerText.text = initTimer.ToString();

        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        float timer = initTimer;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            InfoManager.Instance.timer = timer;
            timerText.text = ((int)timer).ToString();
            yield return null;
        }
        timerText.text = "time over";
    }

}
