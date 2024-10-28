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
    private bool isClose;

    [SerializeField] private GameObject endingUI;
    [SerializeField] private UnityEngine.UI.Button endingButton;

    private void Start()
    {
        endingButton.onClick.AddListener(() => SceneManager_BJH.Instance.ChangeScene("Login"));
        endingUI.SetActive(false);
        topUIGroup.SetActive(true);
        nicknameText.text = GameManager.Instance.nickName;
        GameManager.Instance.coin = 100;
        GameManager.Instance.cash = 0;
        coinText.text = GameManager.Instance.coin.ToString() + " ¿ø";
        cashText.text = GameManager.Instance.cash.ToString() + " ¿ø";
        initTimer = GameManager.Instance.timer;
        timerText.text = initTimer.ToString();

        StartCoroutine(CountDown());
    }

    private void Update()
    {
        if(isClose)
        {
            endingUI.SetActive(true);
        }
    }

    private IEnumerator CountDown()
    {
        float timer = initTimer;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            GameManager.Instance.timer = timer;
            timerText.text = ((int)timer).ToString();
            yield return null;
        }
        timerText.text = "time over";
        isClose = true;
    }

}
