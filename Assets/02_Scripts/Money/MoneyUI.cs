using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [Header("MoneyUI")]
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("CashUI")]
    [SerializeField] private TextMeshProUGUI cashText;

    [Header("NotFillUI")]
    [SerializeField] GameObject notFillUI;

    public void ChangeMoneyUI(int value)
    {
        moneyText.text = value.ToString();
    }

    public void ChangeCashUI(int value)
    {
        cashText.text = value.ToString();
    }

    public void NoMoneyUI()
    {
        StartCoroutine(NotFillMoney());
    }

    IEnumerator NotFillMoney()
    {
        notFillUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        notFillUI.SetActive(false);
    }
}
