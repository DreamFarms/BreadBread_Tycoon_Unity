using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleTest : MonoBehaviour
{
    [SerializeField] GoogleLoginConnection connection;


    public void LoginInGPGS()
    {

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestServerAuthCode(false)
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("�α��� ����");
                var idToken = PlayGamesPlatform.Instance.GetIdToken();
                InfoManager.Instance.SetGoogleToken(idToken);
                connection.StartGoogleLoginConnection();
                Debug.Log($"ID Token: {idToken}");
            }
            else
            {
                Debug.LogError("�α��� ����");
            }
        });
    }

    public void LogoutFromGPGS()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.SignOut();
            Debug.Log("Google Play Games �α׾ƿ� �Ϸ�.");
        }
        else
        {
            Debug.Log("�α׾ƿ��� ����ڰ� �����ϴ�.");
        }
    }

    public void ClickLogin()
    {
        if (InfoManager.Instance.googleToken != null)
        {
            SceneManager_BJH.Instance.ChangeScene("Map");
            Debug.Log("inHere");
        }
        else
        {
            LoginInGPGS();
            Debug.Log("Login One More");
        }
    }
}
