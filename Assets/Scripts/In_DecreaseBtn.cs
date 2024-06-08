using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ���� ��ư�� ������Ʈ�� �߰��Ǵ� Ŭ����
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

    // ���� �Ǻ�
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
        Debug.Log("������");

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
