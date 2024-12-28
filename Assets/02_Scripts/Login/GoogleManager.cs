//using GooglePlayGames.BasicApi;
//using GooglePlayGames;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEditor;
//using System.IO;

//public class GoogleManager : MonoBehaviour
//{
//    public TextMeshProUGUI loginText;
//    public TextMeshProUGUI loginText2;

//    private void Start()
//    {
//        PlayGamesPlatform.DebugLogEnabled = true;
//        PlayGamesPlatform.Activate();
//        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
//    }

//    internal void ProcessAuthentication(SignInStatus status)
//    {
//        if (status == SignInStatus.Success)
//        {
//            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
//            string id = PlayGamesPlatform.Instance.GetUserId();
//            string Imgurl = PlayGamesPlatform.Instance.GetUserImageUrl();
//            loginText2.text = name;
//        }
//        else if (status == SignInStatus.InternalError)
//        {
//            loginText.text = "error";
//        }
//        else if (status == SignInStatus.Canceled)
//        {
//            loginText.text = "cancle";
//        }
//        else 
//        {
//            loginText.text = "Login Fail2";
//        }
//    }

//    public void PushTest()
//    {
//        if (Social.localUser.authenticated)
//        {
//            PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>
//            {
//                if (!string.IsNullOrEmpty(code))
//                {
//                    loginText.text = "Code: " + code; // UI¿¡ Ç¥½Ã
//                    string path = Application.persistentDataPath + "/auth_code.txt";
//                    File.WriteAllText(path, "Code: " + code);
//                }
//                else
//                {
//                    loginText.text = "AuthoFail";
//                }
//            });
//        }
//        else
//        {

//            loginText.text = "login Fail";
//        }
//    }
//}
