using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class LoginUIManager : MonoBehaviour
{
    private static LoginUIManager _instance;

    public static LoginUIManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] public TMP_Text nickNameinput;
    [SerializeField] public Button loginButton;
    [SerializeField] public GameObject alertImageGo;
    [SerializeField] public TMP_Text alertText;
    [SerializeField] public Button alertButton;
    [SerializeField] public Button alerBackButton;

    private void OnEnable()
    {
        loginButton.onClick.AddListener(() => LoginManager.Instance.LoginRequest(nickNameinput.text));
        alertImageGo.SetActive(false);

        alertButton.onClick.AddListener(() => SceneController.Instance.ChangeScene("Main"));
        alerBackButton.onClick.AddListener(() => onClickAlerBackButton());
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.Login);
    }

    private void onClickAlerBackButton()
    {
        alertImageGo.SetActive(false);
        nickNameinput.text = "";
    }

 

}
