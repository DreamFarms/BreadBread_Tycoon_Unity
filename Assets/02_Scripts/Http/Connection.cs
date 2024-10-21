using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Connection : MonoBehaviour
{
    private string _url;
    public string Url
    {
        get { return _url; }
    }

    // public abstract void RewordRequest(); // 리워드를 요청하는 추상 메서드
    // 빵 통신은 전/후로 나뉘어지기 때문에, 위 메서드를 정의하는 것에 대해선 고민이 필요함
}
