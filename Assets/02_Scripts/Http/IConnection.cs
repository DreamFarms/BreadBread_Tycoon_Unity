using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IConnection
{
    public void OnComplete<T>(DownloadHandler result) where T : new();
    public void OnFailed(DownloadHandler result);
}
