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

    public string selectedBreadName; // ���� ��

    // ���� �� ���� ����
    public Dictionary<string, int> targetIngredientDic = new Dictionary<string, int>();

    // ������ ���� ���� ����
    public Dictionary<string, int> userIngredientDic = new Dictionary<string, int>();

    [Header("�����̴�")]
    [SerializeField] private Slider mercurySlider;
    [SerializeField] private float decreaseDuration; // �پ��� �� �ɸ��� �ð�

    [SerializeField] private float decreaseRate; // ���� �ӵ�
    [SerializeField] private float increaseAmout; // �����ϴ� ��


    private void Update()
    {
        // ��ä�� �����ϸ� ����Ǵ� �κ�
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

    // ������ ���۵� �� ȣ��Ǵ� �޼���
    public void StartBakeBreadGame()
    {
        mercurySlider.value = mercurySlider.maxValue;
        StartCoroutine(CoStartBakeBreadGame());
    }

    private IEnumerator CoStartBakeBreadGame()
    {
        float startVlaue = mercurySlider.value;
        float targetVlaue = 0f;
        float time = 0f; // ��� �ð�
        while(time < decreaseDuration)
        {
            mercurySlider.value = Mathf.Lerp(startVlaue, 0, time / decreaseDuration);
            time += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }
        Debug.Log("���� ��");
    }
}
