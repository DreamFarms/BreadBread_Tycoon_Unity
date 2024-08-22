using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    public Camera mainCamera;
    [SerializeField] private LayerMask targetLayer;

    public string selectedBreadName; // ���� ��

    // ���� �� ���� ����
    public Dictionary<string, int> targetIngredientDic = new Dictionary<string, int>();

    // ������ ���� ���� ����
    public Dictionary<string, int> userIngredientDic = new Dictionary<string, int>();

    [Header("�����̴�")]
    [SerializeField] private UnityEngine.UI.Slider mercurySlider;
    [SerializeField] private float totalDuration; // ���� �÷��� �ð�
    [SerializeField] private float decreaseDuration; // �پ��� �� �ɸ��� �ð�
    [SerializeField] private float increaseAmout; // �����ϴ� ��

    private bool isCoRunningMercury;
    private bool isCoRunningTimer;

    [Header("��ä")]
    [SerializeField] private GameObject fan01;
    [SerializeField] private GameObject fan02;

    [Header("��")]
    [SerializeField] private SpriteRenderer[] doughImage;
    [SerializeField] private Sprite[] doughSprite;
    [SerializeField] private int[] indexArray;
    [SerializeField] private int bakedBreadCount;

    [Header("������")]
    [SerializeField] private GameObject rewordGO;
    [SerializeField] private GameObject rewordBGImageGO;
    [SerializeField] private UnityEngine.UI.Image rewordItemImage;
    [SerializeField] private TMP_Text rewordCountText;
    [SerializeField] private CustomButton rewordCheckButton;






    public bool isPlay;

    private void Start()
    {
        fan02.SetActive(false);
        rewordGO.SetActive(false);
        rewordCountText.text = "0";
        rewordCheckButton.onClick.AddListener(() => SceneManager_BJH.Instance.ChangeScene("Map"));

        indexArray = new int[doughImage.Length];
        for (int i = 0; i < indexArray.Length; i++)
        {
            indexArray[i] = 0;
        }
    }

    private void Update()
    {
        if(isPlay)
        {
            // ���콺 Ŭ�� ����
            if (Input.GetMouseButtonDown(0)) // ���� ���콺 ��ư Ŭ��
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, targetLayer);

                // ��ä �ݶ��̴� �κ��� Ŭ�� �� ���
                if(hit.collider.gameObject.name == "FanCollider")
                {
                    IncreaseTemperature();
                    ChangePanImage();
                }

                // ���츦 Ŭ�� �� ���
                if(hit.collider.gameObject.name.Contains("Dough"))
                {
                    Debug.Log($"{hit.collider.gameObject.name} �� Ŭ���߽��ϴ�.");
                    if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Dough03")
                    {
                        // �� ��ȣ Ȯ�� ex) dough01�� 1�� ����
                        int lastNum = int.Parse((hit.collider.gameObject.name[hit.collider.gameObject.name.Length - 1]).ToString());
                        indexArray[lastNum - 1] = 0;
                        doughImage[lastNum - 1].sprite = doughSprite[indexArray[lastNum - 1]];

                        // �ش� index�� dough�� 0���� �ʱ�ȭ
                        int temp = indexArray[lastNum - 1];
                        temp++;
                        indexArray[lastNum - 1] = temp;

                        // �� ��� �����ؼ� �� ����
                        foreach(var target in targetIngredientDic)
                        {
                            string targetName = target.Key;
                            int targetCount = target.Value;

                            int remain = userIngredientDic[targetName] - targetCount;

                            // ��ᰡ �ϳ��� �����ϸ� ���� ���̻� ���� �� ����.
                            if(remain <= 0)
                            {
                                Debug.Log("���� �� ��ᰡ �����մϴ�.");
                                isPlay = false; // ������ ������.
                                break;
                            }

                            userIngredientDic[targetName] = remain;

                        }
                        bakedBreadCount++;
                    }
                }
            }
        }
    }




    private void ChangePanImage()
    {
        fan01.SetActive(!fan01.activeSelf);
        fan02.SetActive(!fan02.activeSelf); 

    }

    private void IncreaseTemperature()
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
        isPlay = true;
        mercurySlider.value = mercurySlider.maxValue;
        StartCoroutine(CoStartBakeBreadGame());
        StartCoroutine(CoStartTimer());
        StartCoroutine(CoBakeDough());
    }

    private IEnumerator CoStartBakeBreadGame()
    {
        isCoRunningMercury = true;
        while (mercurySlider.value > 0)
        {
            // �����̴� ���� �ڿ������� ����
            mercurySlider.value -= Time.deltaTime / decreaseDuration;

            // �����̴� ���� 0 ���Ϸ� �������� �ʵ��� ó��
            if (mercurySlider.value <= 0)
            {
                mercurySlider.value = 0;
                EndGame();
                StopCoroutine(CoStartBakeBreadGame());
            }

            yield return null; // ���� �����ӱ��� ���
        }
        isCoRunningMercury = false;

    }

    private IEnumerator CoStartTimer()
    {
        isCoRunningTimer = true;
        yield return new WaitForSeconds(totalDuration); // ������ �ð����� ���
        isCoRunningTimer = false;
        Debug.Log("�ڷ�ƾ ��");
        EndGame();
    }

    private IEnumerator CoBakeDough()
    {

        while(isPlay)
        {
            // ��
            for(int i = 0; i < doughImage.Length; i++)
            {
                if (indexArray[i] >= 4)
                {
                    indexArray[i] = 0;
                }
                doughImage[i].sprite = doughSprite[indexArray[i]];
                int temp = indexArray[i];
                temp++;
                indexArray[i] = temp;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private void EndGame()
    {
        if(isCoRunningMercury)
        {
            StopCoroutine(CoStartBakeBreadGame());
            isCoRunningMercury = false;
        }

        if(isCoRunningTimer)
        {
            StopCoroutine(CoStartTimer());
            isCoRunningTimer = false;
        }

        isPlay = false;
        Debug.Log("���� ��");
        Debug.Log($"�� ������ ���� ������ {bakedBreadCount}�Դϴ�.");

        // ���
        BakeBreadConnection connection = GameObject.Find("BakeBreadConnection").GetComponent<BakeBreadConnection>();
        connection.EndBakeBread(selectedBreadName, userIngredientDic, bakedBreadCount);

        // UI ȣ��
        rewordGO.SetActive(true);
        rewordBGImageGO.SetActive(true);
        rewordItemImage.sprite = Resources.Load<Sprite>(selectedBreadName);
        rewordCountText.text = bakedBreadCount.ToString();


    }
}
