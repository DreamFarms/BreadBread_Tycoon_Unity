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
    [SerializeField] private Transform rewardItemTr;
    [SerializeField] private Button reStartBtn;
    [SerializeField] private Button exitBtn;

    [SerializeField] private GameObject failRewardPrefab;
    [SerializeField] private Button failReStartBtn;
    [SerializeField] private Button failExitBtn;





    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        // tutorial.SetActive(true);
        // startButton.GetComponent<Button>().onClick.AddListener(OnClickStartButton);
        
        rewardBG.SetActive(false);

        foreach(GameObject go in oxSprites)
        {
            go.SetActive(false);
        }

        foreach(GameObject go in countNumbers)
        {
            go.SetActive(false);
        }

        reStartBtn.onClick.AddListener(() => MilkGameManager.Instance.StartGame());
        exitBtn.onClick.AddListener(() => SceneController.Instance.ChangeScene(SceneName.Map));

        failReStartBtn.onClick.AddListener(() => MilkGameManager.Instance.StartGame());
        failExitBtn.onClick.AddListener(() => SceneController.Instance.ChangeScene(SceneName.Map));
    }

    private void OnClickStartButton()
    {
        tutorial.SetActive(false);
        MilkGameManager.Instance.StartGame();
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
        if(itemCount <= 0)
        {
            failRewardPrefab.SetActive(true);
        }

        rewardBG.SetActive(true);

        GameObject rewardItemGo = Instantiate(rewardItemPrefab, rewardItemTr);
        rewardItemGo.SetActive(true);

        RewardUI rewardUI = rewardItemGo.GetComponent<RewardUI>();
        rewardUI.itemImage.sprite = rewordSprite;
        rewardUI.countText.text = itemCount + "개";

        // 통신 여기에 있는게 맞나?
        // MilkGameManager.Instance.RequestConnection();
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

    public void SwitchTutorialUI()
    {
        tutorial.SetActive(!tutorial.activeSelf);
    }

    public void InitUI()
    {
        tutorial.SetActive(false);
        rewardBG.SetActive(false);
        failRewardPrefab.SetActive(false);

        foreach (Transform tr in rewardItemTr)
        {
            Destroy(tr.gameObject);
        }
    }

}
