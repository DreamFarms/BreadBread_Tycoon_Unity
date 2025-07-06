using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickNameSetting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nickNametext;

    [SerializeField] private GameObject errorUI;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private NickNameConnection connection;

    public void SetNickName()
    {
        if (nickNametext.text.Length < 0 && nickNametext.text.Length > 8)
        {
            errorText.text = "�г����� ���ڼ��� ���� �ʽ��ϴ�";
            errorUI.SetActive(true);
            return;
        }

        connection.StartNickNameConnection(nickNametext.text);
    }
}
