using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

// ����Ʈ �ڵ�� Notion `����Ʈ - ����Ƽ�� ������ DB` ȸ�Ƿ��� ����

[SerializeField]
public class QuestRequest
{
    public string nickname; // �г���
    public long questCode; // ����Ʈ �ڵ�
}

[SerializeField]
public class QuestResponse
{
    public string resultCode;
    public QuestResponseMessage message;
}

[SerializeField]
public class QuestResponseMessage
{
    public long questCode; // ����Ʈ �ڵ�
    public int completedCount; // �Ϸ� Ƚ��
}

public class QuestConnection : MonoBehaviour
{
    [SerializeField] private string endPoint = "api/v1/quest/complete";

    private void Start()
    {
        QuestRequest("000", 3000);
    }

    public void QuestRequest(string nickname, long questCode)
    {
        QuestRequest request = new QuestRequest();

        request.nickname = nickname;
        request.questCode = questCode;

        string json = JsonUtility.ToJson(request);

        string url = GameManager.Instance.Url + endPoint;
        Debug.Log(url);
        HttpRequester requester = new HttpRequester(RequestType.POST, url, json);
        requester.onComplete = OnComplete;

        HttpManager.Instance.SendRequest(requester);
    }

    public void OnComplete(DownloadHandler handler)
    {
        QuestResponse response = JsonUtility.FromJson<QuestResponse>(handler.text);

        Debug.Log($"����Ʈ ����� �Ϸ��߽��ϴ�. \n ��� ��� : {response.resultCode}");
        Debug.Log($"����Ʈ �ڵ� : {response.message.questCode} \n ����Ʈ �Ϸ� Ƚ�� : {response.message.completedCount}");
    }

    public void OnFailed(DownloadHandler handler)
    {
        Debug.LogError("����Ʈ ��ſ� �����߽��ϴ�.");
    }
}
