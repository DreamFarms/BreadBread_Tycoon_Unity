using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private List<GameObject> _activeUIList = new List<GameObject>();

    public Image bg;

    public GameObject selectPlateMenuImg;
    public GameObject breadInfoImgGroup;
    public TextMeshProUGUI menuKoName;
    public TextMeshProUGUI menuPrice;
    public TextMeshProUGUI menuDescription;

    public GameObject selectMenuCountingImg;
    public GameObject selectedMenuImg;
    public TextMeshProUGUI menuCountingTxt;

    public GameObject sideDirectionArrowBtn;

    // 사이드 화살표 버튼이 활성화되는 UI리스트
    private List<GameObject> _needSideBtnUILIst = new List<GameObject>();

    public Button mapButton;

    [Header("bake bread")]
    public GameObject blackBGImage;
    public GameObject errorImage;
    public string selectedBreadName;
    public Slider thermometer;




    private void Start()
    {
        if(SceneManager.GetActiveScene().name.Equals("Game_BakeBread"))
        {
            // bake bread
            blackBGImage.SetActive(true);

            BlackBGImage blackBGImageComponent = blackBGImage.GetComponent<BlackBGImage>();
            blackBGImageComponent.CloseButton.onClick.AddListener(() => SceneManager_BJH.Instance.ChangeScene("Map")); // 게임 나가야 함
            blackBGImageComponent.nextButton.onClick.AddListener(() => GameObject.Find("BakeBreadConnection").GetComponent<BakeBreadConnection>().StartBakeBreadConnection()); // 통신 해야 함

            errorImage.GetComponent<ErrorImage>().errorImageButton.onClick.AddListener(() => CloseUI(errorImage));
            errorImage.SetActive(false);


        }
        else // 임시
        {
            // 사이드 화살표가 필요한 UI들은 리스트에 추가
            _needSideBtnUILIst.Add(selectedMenuImg);

            bg.enabled = false;
            selectPlateMenuImg.SetActive(false);
            breadInfoImgGroup.SetActive(false);
            selectMenuCountingImg.SetActive(false);
            sideDirectionArrowBtn.SetActive(false);

            mapButton.onClick.AddListener(() => SceneManager_BJH.Instance.ChangeScene("Map"));

            // 메뉴 설명 기본 문구 초기화
            SetBasicMenuInfoText();
        }

    }

    public void SetErrorImage(string text)
    {
        ErrorImage errorImageComponent = errorImage.GetComponent<ErrorImage>();
        errorImageComponent.errorText.text = text;
    }

    public void SetErrorImage(string text, string buttonText)
    {
        ErrorImage errorImageComponent = errorImage.GetComponent<ErrorImage>();
        errorImageComponent.errorText.text = text;
        errorImageComponent.errorButtonText.text = buttonText;
    }
    private void CloseUI(GameObject target)
    {
        target.SetActive(false);
    }

    private void SetBasicMenuInfoText()
    {
        menuKoName.text = "";
        menuPrice.text = "";
        menuDescription.text = "원하는 메뉴를 진열해보세요!";
    }

    public void SetSlectedMenuInfoText(string koName, string price, string description)
    {
        menuKoName.text = koName;
        menuPrice.text = int.Parse(price).ToString("N0") + "원";
        menuDescription.text = description;
    }

    
    // UI 활성화
    public void OnActiveThisUI(GameObject targetUI)
    {
        targetUI.SetActive(true);

        foreach(GameObject go in _activeUIList)
        {
            if(go.name == targetUI.name)
            {
                Debug.Log("사이드 버튼이 활성화 되야합니다.");
                sideDirectionArrowBtn.SetActive(true);
            }
        }

        // bg 활성화
        EnableBG(true);
        
        OnAddActiveUIList(targetUI);
    }

    // BG 활성, 비활성화
    public void EnableBG(bool state)
    {
        bg.enabled = state;
    }

    // 활성화된 UI를 리스트로 관리
    private void OnAddActiveUIList(GameObject targetUI)
    {
        _activeUIList.Add(targetUI);
    }

    // 활성화된 UI 한꺼번에 비활성화
    public void OnInactiveAllUI()
    {
        var temp = new List<GameObject>(_activeUIList);
        foreach(GameObject targetUI in temp)
        {
            targetUI.SetActive(false);
            _activeUIList.Remove(targetUI);
        }
    }

    public void OnSelectedBreadNameUpdate(Image image)
    {
        BakeBreadManager.Instance.selectedBreadName = image.sprite.name;
    }


}
