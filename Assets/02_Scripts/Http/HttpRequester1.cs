using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum RequestType1
{
    GET, POST, PUT, DELETE
}

public class HttpRequester1 : MonoBehaviour
{
    public RequestType1 requestType;
    public string url;
    public string json = "";
    public Action<DownloadHandler> onComplete;
    public Action<DownloadHandler> onFailed;

    public HttpRequester1(RequestType1 type, string url)
    {
        this.requestType = type;
        this.url = url;
    }

    public HttpRequester1(RequestType1 type, string url, string json)
    {
        this.requestType = type;
        this.url = url;
        this.json = json;
    }

    public void OnComplete(DownloadHandler result)
    {
        if (onComplete != null) onComplete(result);
    }

    public void OnFailed(DownloadHandler result)
    {
        if (onFailed != null) onFailed(result);
    }
}

