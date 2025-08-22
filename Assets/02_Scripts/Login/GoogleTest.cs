using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleTest : MonoBehaviour
{
    [SerializeField] GoogleLoginConnection connection;
    public GameObject nickNameLogin;

    private void Awake()
    {
        //if (InfoManager.Instance.JsonLoad())
        //{
        //    connection.StartGoogleLoginConnection();
        //}
    }

    private void Start()
    {
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
        InfoManager.Instance.SetToken("eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg4MjUwM2E1ZmQ1NmU5ZjczNGRmYmE1YzUwZDdiZjQ4ZGIyODRhZTkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIyNDgyNTI3OTg3MjUtYWlhMnBrMDE3ZGRzcTUzbGI2dmF1dm9qYnVycjUxZmwuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiIyNDgyNTI3OTg3MjUtbTAwbzQ4a2lyaThvZ2JlNmkxaGk3YjVnbjFtOWFoYW4uYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDg4ODA3MzE5MDExNDE0MjE2MzMiLCJpYXQiOjE3NTEwMjU2OTIsImV4cCI6MTc1MTAyOTI5Mn0.tQugk7FzUAeE6Dbf2oGGJGapS3_3jTO-PJ0ZuZr98LE9h4O3P1S75czYzAfKLmeBZ1uqv1nPAf-fzFN_imMJKmXhHBYJtDy0iM9Qa_eFimL-RebCwBYS39SZTr7gT3mR_uB7ojJPXvTl1vNaudOEvLU5xvVmSWV-YjrL1hxUOCwiWfnkWNS_c0hNp4PJks2rqY2_pRfwS_G7LwurpiiPTJ8YdNVazmTc8DUEstssA2ew1vkGmpjFicfzwMxnmo1wFZNjMUhiYKr3andWbfWfeN86Ou2leZu1jQuj37LCUOhvDX-PEwsp2OBaL0_eKtgltqiJDJy6I1hDjYBnfRE-6Q", "gBzEqzdnNWR394ncl12fTRW7uwRIfC7CiDMoO8NhaI2yjJOYHxi92z3fYKLF5GrG8+5l3fbHP9lOTfeTuHGCEENw6ee490xDvx/i1bcvmyb7MSJu+/5bgzdyuwEmFM9zUFVUcIEu5mpduo5mL7G9b7+nlbTln9dPDoqb2m8LpRoOOXuBEyWcdX4DF8bE4O0lLcDOT5CfYN1EtZyBjb2K3jZyWUi08O5VeRYwjjn5RqQ=");
        connection.StartGoogleLoginConnection();
    }
}
