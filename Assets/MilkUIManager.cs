using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilkUIManager : MonoBehaviour
{
    private static MilkUIManager _instance;

    public static MilkUIManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] private GameObject[] oxSprites; // ox 이미지 assign // 0 : false, 1 : true

    [Header("튜토리얼 UI")]
    [SerializeField] private GameObject tutorial; // assign 
    [SerializeField] private GameObject startButton; // assign
    [SerializeField] private GameObject rememberButton; // 서버, 기기에 저장?

    [Header("카운트다운 UI")]
    [SerializeField] private GameObject[] countNumbers;

    [Header("Reward UI")]
    [SerializeField] private GameObject rewardBG;
    [SerializeField] private GameObject rewardItemPrefab;
    [SerializeField] private Sprite rewordSprite;




    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        tutorial.SetActive(true);
        rewardBG.SetActive(false);
        startButton.GetComponent<Button>().onClick.AddListener(OnClickStartButton);

        foreach(GameObject go in oxSprites)
        {
            go.SetActive(false);
        }

        foreach(GameObject go in countNumbers)
        {
            go.SetActive(false);
        }
    }

    private void OnClickStartButton()
    {
        tutorial.SetActive(false);
        MilkGameManager.Instance.ChangeGameState();
    }

    public void StartCountDown()
    {
        StartCoroutine(CoStartCountDown());
    }

    private IEnumerator CoStartCountDown()
    {
        for (int i = 0; i < countNumbers.Length; i++)
        {
            countNumbers[i].SetActive(true);
            yield return new WaitForSeconds(1);
            countNumbers[i].SetActive(false);
        }
    }

    public void ActiveRewardUI(int itemCount)
    {
        // 무한 호출을 방지하기 위해서 UI Manager가 reward 호출
        if(!rewardBG.activeSelf)
        {
            rewardBG.SetActive(true);
            Transform contentTr = rewardBG.transform.GetChild(1);
            GameObject rewardItemGo = Instantiate(rewardItemPrefab, contentTr);
            ItemInfo itemInfo = rewardItemGo.GetComponent<ItemInfo>();
            itemInfo.rewordItemImage.sprite = rewordSprite;
            itemInfo.rewordItemName.text = itemCount + "개";

            MilkGameManager.Instance.RequestConnection();
        }
    }

    public void ActiveO()
    {
        oxSprites[1].SetActive(true);
        StartCoroutine(DeActiveOXUI(oxSprites[1]));

    }

    public void ActiveX()
    {
        oxSprites[0].SetActive(true);
        StartCoroutine(DeActiveOXUI(oxSprites[0]));
    }

    private IEnumerator DeActiveOXUI(GameObject go)
    {
        yield return new WaitForSeconds(0.5f);
        go.SetActive(false);
    }

    

}
