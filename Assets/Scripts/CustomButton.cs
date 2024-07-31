using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    // �ش� ��ư�� Ŭ���ϸ� �Ű����� ������Ʈ ���°� ������
    // on -> off
    // off -> on
    public void Onclick_ChangeUIState(GameObject go)
    {
        bool state = go.activeSelf;
        go.SetActive(!state);
    }
}
