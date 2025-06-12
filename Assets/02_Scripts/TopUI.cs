using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TopUI : MonoBehaviour
{
    public TMP_Text nickName;
    public TMP_Text gold;
    public TMP_Text cash;

    private InfoManager info = InfoManager.Instance;

    private void OnEnable()
    {
        InitTopUI();
        SubEvent();
    }

    private void OnDisable()
    {
        DisSubEvent();
    }

    private void SubEvent()
    {
        info.OnGoldChanged += UpdateGold;
        info.OnCashChanged += UpdateCash;
    }

    private void DisSubEvent()
    {
        info.OnGoldChanged -= UpdateGold;
        info.OnCashChanged -= UpdateCash;
    }

    private void InitTopUI()
    {
        InfoManager info = InfoManager.Instance;
        this.nickName.text = info.NickName;
        this.gold.text = info.Gold.ToString();
        this.cash.text = info.Cash.ToString();

        Debug.Log("TopUI를 초기화 했습니다.");
    }

    private void UpdateGold(int amount)
    {
        this.gold.text = amount.ToString();
        Debug.Log("골드가 추가됐습니다.");
    }

    private void UpdateCash(int amount) 
    { 
        this.cash.text = amount.ToString();
        Debug.Log("캐시가 추가됐습니다.");
    }

    // 테스트 버튼입니다.
    public void OnClickTestButton()
    {
        InfoManager.Instance.AddGold(1000);
    }

}
