using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{
    private static LoginManager _instance;
    public static LoginManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] private LoginUIManager _loginUI;
    [SerializeField] private LoginConnection _loginConnection;

    private void Awake()
    {
        if(_instance == null) { _instance = this; }
    }

    public void LoginRequest(string inputNickname)
    {
        _loginConnection.LoginRequest(inputNickname);
    }
}
