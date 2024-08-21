using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BakeBreadManager : MonoBehaviour
{
    #region _instance
    private static BakeBreadManager _instance;

    public static BakeBreadManager Instance
    {  get { return _instance; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }
    #endregion

    public string selectedBreadName; // 구울 빵

    // 구울 빵 재료와 개수
    public Dictionary<string, int> targetIngredientDic = new Dictionary<string, int>();

    // 유저가 가진 재료와 개수
    public Dictionary<string, int> userIngredientDic = new Dictionary<string, int>();

    [Header("슬라이더")]
    [SerializeField] private Slider mercurySlider;
    [SerializeField] private float decreaseDuration; // 줄어드는 데 걸리는 시간

    [SerializeField] private float decreaseRate; // 감소 속도
    [SerializeField] private float increaseAmout; // 증가하는 양


    private void Update()
    {
        // 부채를 감지하면 실행되는 부분
        //IncreaseSliderValue();
    }

    private void IncreaseSliderValue()
    {
        mercurySlider.value += increaseAmout;
        if(mercurySlider.value > mercurySlider.maxValue )
        {
            mercurySlider.value = mercurySlider.maxValue;
        }
    }

    // 게임이 시작될 때 호출되는 메서드
    public void StartBakeBreadGame()
    {
        mercurySlider.value = mercurySlider.maxValue;
        StartCoroutine(CoStartBakeBreadGame());
    }

    private IEnumerator CoStartBakeBreadGame()
    {
        float startVlaue = mercurySlider.value;
        float targetVlaue = 0f;
        float time = 0f; // 경과 시간
        while(time < decreaseDuration)
        {
            mercurySlider.value = Mathf.Lerp(startVlaue, 0, time / decreaseDuration);
            time += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }
        Debug.Log("게임 끝");
    }
}
