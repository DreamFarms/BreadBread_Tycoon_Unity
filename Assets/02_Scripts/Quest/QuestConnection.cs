using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

// 퀘스트 코드는 Notion `퀘스트 - 유니티와 서버의 DB` 회의록을 참고

[SerializeField]
public class QuestRequest
{
    public string nickname; // 닉네임
    public long questCode; // 퀘스트 코드
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
    public long questCode; // 퀘스트 코드
    public int completedCount; // 완료 횟수
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

        Debug.Log($"퀘스트 통신을 완료했습니다. \n 통신 결과 : {response.resultCode}");
        Debug.Log($"퀘스트 코드 : {response.message.questCode} \n 퀘스트 완료 횟수 : {response.message.completedCount}");
    }

    public void OnFailed(DownloadHandler handler)
    {
        Debug.LogError("퀘스트 통신에 실패했습니다.");
    }
}
