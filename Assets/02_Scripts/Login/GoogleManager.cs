using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{
    public TextMeshProUGUI loginText;

    private void Start()
    {
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();
        
        SignIn();
    }

    public void SignIn()
    {
        //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            //string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            //string id = PlayGamesPlatform.Instance.GetUserId();
            //string Imgurl = PlayGamesPlatform.Instance.GetUserImageUrl();

            loginText.text = "Succes :" + name;
        }
        else
        {
            loginText.text = "Login Fail";
        }
    }
}
