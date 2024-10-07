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

        foreach(GameObject go in oxSprites)
        {
            go.SetActive(false);
        }
    }

    private void Start()
    {
        startButton.GetComponent<Button>().onClick.AddListener(OnClickStartButton);
    }

    private void OnClickStartButton()
    {
        tutorial.SetActive(false);
        MilkGameManager.Instance.ChangeGameState();
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
