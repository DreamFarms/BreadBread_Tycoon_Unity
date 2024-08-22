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

    // ���̵� ȭ��ǥ ��ư�� Ȱ��ȭ�Ǵ� UI����Ʈ
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
            blackBGImageComponent.CloseButton.onClick.AddListener(() => SceneManager_BJH.Instance.ChangeScene("Map")); // ���� ������ ��
            blackBGImageComponent.nextButton.onClick.AddListener(() => GameObject.Find("BakeBreadConnection").GetComponent<BakeBreadConnection>().StartBakeBreadConnection()); // ��� �ؾ� ��

            errorImage.GetComponent<ErrorImage>().errorImageButton.onClick.AddListener(() => CloseUI(errorImage));
            errorImage.SetActive(false);


        }
        else // �ӽ�
        {
            // ���̵� ȭ��ǥ�� �ʿ��� UI���� ����Ʈ�� �߰�
            _needSideBtnUILIst.Add(selectedMenuImg);

            bg.enabled = false;
            selectPlateMenuImg.SetActive(false);
            breadInfoImgGroup.SetActive(false);
            selectMenuCountingImg.SetActive(false);
            sideDirectionArrowBtn.SetActive(false);

            mapButton.onClick.AddListener(() => SceneManager_BJH.Instance.ChangeScene("Map"));

            // �޴� ���� �⺻ ���� �ʱ�ȭ
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
        menuDescription.text = "���ϴ� �޴��� �����غ�����!";
    }

    public void SetSlectedMenuInfoText(string koName, string price, string description)
    {
        menuKoName.text = koName;
        menuPrice.text = int.Parse(price).ToString("N0") + "��";
        menuDescription.text = description;
    }

    
    // UI Ȱ��ȭ
    public void OnActiveThisUI(GameObject targetUI)
    {
        targetUI.SetActive(true);

        foreach(GameObject go in _activeUIList)
        {
            if(go.name == targetUI.name)
            {
                Debug.Log("���̵� ��ư�� Ȱ��ȭ �Ǿ��մϴ�.");
                sideDirectionArrowBtn.SetActive(true);
            }
        }

        // bg Ȱ��ȭ
        EnableBG(true);
        
        OnAddActiveUIList(targetUI);
    }

    // BG Ȱ��, ��Ȱ��ȭ
    public void EnableBG(bool state)
    {
        bg.enabled = state;
    }

    // Ȱ��ȭ�� UI�� ����Ʈ�� ����
    private void OnAddActiveUIList(GameObject targetUI)
    {
        _activeUIList.Add(targetUI);
    }

    // Ȱ��ȭ�� UI �Ѳ����� ��Ȱ��ȭ
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
