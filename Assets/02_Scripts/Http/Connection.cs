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

    // public abstract void RewordRequest(); // �����带 ��û�ϴ� �߻� �޼���
    // �� ����� ��/�ķ� ���������� ������, �� �޼��带 �����ϴ� �Ϳ� ���ؼ� ����� �ʿ���
}
