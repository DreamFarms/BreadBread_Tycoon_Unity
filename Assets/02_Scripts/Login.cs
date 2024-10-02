using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class LoginRequest
{
    public string nickname;
}

[Serializable]
public class LoginResponse
{
    public string resultcode;
    public LoginMessage message;
}

[Serializable]
public class LoginMessage
{
    public string nickname;
    public string message;
}

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_Text nickNameinput;
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject alertImageGo;
    [SerializeField] private TMP_Text alertText;
    [SerializeField] private Button alertButton;
    [SerializeField] private Button alerBackButton;


    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.Login);
        loginButton.onClick.AddListener(LoginRequest);
        alertImageGo.SetActive(false);
        alertButton.onClick.AddListener(() => SceneManager_BJH.Instance.ChangeScene("Main"));
        alerBackButton.onClick.AddListener(() => onClickAlerBackButton());
    }

    private void onClickAlerBackButton()
    {
        alertImageGo.SetActive(false);
        nickNameinput.text = "";
    }

    private void LoginRequest()
    {
        Debug.Log("�α��� ��� ����");
        LoginRequest request = new LoginRequest();
        request.nickname = nickNameinput.text;
        Debug.Log(request.nickname);

        string json = JsonUtility.ToJson(request);

        string url = "http://ec2-3-35-69-253.ap-northeast-2.compute.amazonaws.com:8081/api/v1/user/save";
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete;
        requester.onFailed = OnFailed;

        HttpManager.Instance.SendRequest(requester);

    }
    private void OnComplete(DownloadHandler handler)
    {
        Debug.Log("�α��� �Ϸ�");
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(handler.text);

        InfoManager.Instance.nickName = response.message.nickname;

        alertImageGo.SetActive(true);
        // �α����̶��?
        if(response.message.message.Contains("Login"))
        {
            alertText.text = "�����ϴ� ����\n ������ �´ٸ� �α���\n �ƴ϶�� ȸ�������ϼ���.";
            alerBackButton.gameObject.SetActive(true);

        }
        else
        {
            alertText.text = "�������� �ʴ� ����\n ȸ������ �մϴ�.";
            alerBackButton.gameObject.SetActive(false);
        }
        
    }

    private void OnFailed(DownloadHandler handler)
    {
        Debug.Log("�α��� ����");
    }

}
