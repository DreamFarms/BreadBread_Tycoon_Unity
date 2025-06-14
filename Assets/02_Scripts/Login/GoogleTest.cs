using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleTest : MonoBehaviour
{
    [SerializeField] GoogleLoginConnection connection;

    private void Awake()
    {
        //if (InfoManager.Instance.JsonLoad())
        //{
        //    connection.StartGoogleLoginConnection();
        //}
#if UNITY_ANDROID
        LoginInGPGS();
#endif

#if UNITY_EDITOR
        LoginInUnity();
    #endif
    }

#if UNITY_ANDROID
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
                Debug.Log("로그인 성공");
                var idToken = PlayGamesPlatform.Instance.GetIdToken();
                InfoManager.Instance.SetGoogleToken(idToken);
                connection.StartGoogleLoginConnection();
                Debug.Log($"ID Token: {idToken}");
            }
            else
            {
                Debug.LogError("로그인 실패");
            }
        });
    }

    public void LogoutFromGPGS()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.SignOut();
            Debug.Log("Google Play Games 로그아웃 완료.");
        }
        else
        {
            Debug.Log("로그아웃할 사용자가 없습니다.");
        }
    }

    public void ClickLogin()
    {
        if (InfoManager.Instance.googleToken != null)
        {
            Debug.Log("inHere");
        }
        else
        {
            LoginInGPGS();
            Debug.Log("Login One More");
        }
    }
#endif
    public void LoginInUnity()
    {
        InfoManager.Instance.SetToken(InfoManager.Instance.accessTokenTest, InfoManager.Instance.refreshTokenTest);
    }
}
