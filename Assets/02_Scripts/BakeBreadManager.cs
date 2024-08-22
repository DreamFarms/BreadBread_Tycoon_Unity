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

    public string selectedBreadName; // 구울 빵

    // 구울 빵 재료와 개수
    public Dictionary<string, int> targetIngredientDic = new Dictionary<string, int>();

    // 유저가 가진 재료와 개수
    public Dictionary<string, int> userIngredientDic = new Dictionary<string, int>();

    [Header("슬라이더")]
    [SerializeField] private UnityEngine.UI.Slider mercurySlider;
    [SerializeField] private float totalDuration; // 게임 플레이 시간
    [SerializeField] private float decreaseDuration; // 줄어드는 데 걸리는 시간
    [SerializeField] private float increaseAmout; // 증가하는 양

    private bool isCoRunningMercury;
    private bool isCoRunningTimer;

    [Header("부채")]
    [SerializeField] private GameObject fan01;
    [SerializeField] private GameObject fan02;

    [Header("빵")]
    [SerializeField] private SpriteRenderer[] doughImage;
    [SerializeField] private Sprite[] doughSprite;
    [SerializeField] private int[] indexArray;
    [SerializeField] private int bakedBreadCount;

    [Header("리워드")]
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
            // 마우스 클릭 감지
            if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼 클릭
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, targetLayer);

                // 부채 콜라이더 부분을 클릭 할 경우
                if(hit.collider.gameObject.name == "FanCollider")
                {
                    IncreaseTemperature();
                    ChangePanImage();
                }

                // 도우를 클릭 할 경우
                if(hit.collider.gameObject.name.Contains("Dough"))
                {
                    Debug.Log($"{hit.collider.gameObject.name} 을 클릭했습니다.");
                    if(hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Dough03")
                    {
                        // 뒷 번호 확인 ex) dough01의 1을 추출
                        int lastNum = int.Parse((hit.collider.gameObject.name[hit.collider.gameObject.name.Length - 1]).ToString());
                        indexArray[lastNum - 1] = 0;
                        doughImage[lastNum - 1].sprite = doughSprite[indexArray[lastNum - 1]];

                        // 해당 index의 dough를 0으로 초기화
                        int temp = indexArray[lastNum - 1];
                        temp++;
                        indexArray[lastNum - 1] = temp;

                        // 빵 재료 차감해서 값 저장
                        foreach(var target in targetIngredientDic)
                        {
                            string targetName = target.Key;
                            int targetCount = target.Value;

                            int remain = userIngredientDic[targetName] - targetCount;

                            // 재료가 하나라도 부족하면 빵을 더이상 만들 수 없다.
                            if(remain <= 0)
                            {
                                Debug.Log("소진 될 재료가 부족합니다.");
                                isPlay = false; // 게임을 끝낸다.
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

    // 게임이 시작될 때 호출되는 메서드
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
            // 슬라이더 값을 자연스럽게 감소
            mercurySlider.value -= Time.deltaTime / decreaseDuration;

            // 슬라이더 값이 0 이하로 떨어지지 않도록 처리
            if (mercurySlider.value <= 0)
            {
                mercurySlider.value = 0;
                EndGame();
                StopCoroutine(CoStartBakeBreadGame());
            }

            yield return null; // 다음 프레임까지 대기
        }
        isCoRunningMercury = false;

    }

    private IEnumerator CoStartTimer()
    {
        isCoRunningTimer = true;
        yield return new WaitForSeconds(totalDuration); // 지정된 시간동안 대기
        isCoRunningTimer = false;
        Debug.Log("코루틴 끝");
        EndGame();
    }

    private IEnumerator CoBakeDough()
    {

        while(isPlay)
        {
            // 빵
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
        Debug.Log("게임 끝");
        Debug.Log($"총 구워진 빵의 개수를 {bakedBreadCount}입니다.");

        // 통신
        BakeBreadConnection connection = GameObject.Find("BakeBreadConnection").GetComponent<BakeBreadConnection>();
        connection.EndBakeBread(selectedBreadName, userIngredientDic, bakedBreadCount);

        // UI 호출
        rewordGO.SetActive(true);
        rewordBGImageGO.SetActive(true);
        rewordItemImage.sprite = Resources.Load<Sprite>(selectedBreadName);
        rewordCountText.text = bakedBreadCount.ToString();


    }
}
