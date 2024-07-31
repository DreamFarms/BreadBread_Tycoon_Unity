using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 증감 버튼에 컴포넌트로 추가되는 클래스
public class In_DecreaseBtn : MonoBehaviour
{
    Button _btn;
    private string _increase = "IncreaseBtn";
    private string _decrease = "DecreaseBtn";
    private int _minValue = 0;
    private int _maxValue = 99;

    private void Start()
    {
        _btn = GetComponent<Button>();
        Determine();
    }

    // 증감 판별
    private void Determine()
    {
        if(gameObject.name == _increase)
        {
            _btn.onClick.AddListener(IncreaseNumber);
        }
        else if(gameObject.name == _decrease)
        {
            _btn.onClick.AddListener(DecreaseNumber);
        }
    }

    private void IncreaseNumber()
    {
        string original = UIManager.Instance.menuCountingTxt.text;
        int num = int.Parse(original);

        if (_maxValue <= num)
        {
            return;
        }

        Debug.Log("num");
        num++;
        Debug.Log("num");
        UIManager.Instance.menuCountingTxt.text = num.ToString();
        Debug.Log(UIManager.Instance.menuCountingTxt.text);
    }

    private void DecreaseNumber()
    {
        Debug.Log("감소중");

        string original = UIManager.Instance.menuCountingTxt.text;
        int num = int.Parse(original);

        if(_minValue >= num)
        {
            return;
        }

        num--;
        UIManager.Instance.menuCountingTxt.text = num.ToString();
    }


}
