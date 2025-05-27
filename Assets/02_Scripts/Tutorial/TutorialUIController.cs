using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    public GameObject targetUI;

    public void ShowUI()
    {
        targetUI.SetActive(true);
    }

    public void HideUI()
    {
        targetUI.SetActive(false);
    }

    public void ShowText(string text)
    {
        Debug.Log("UI �ؽ�Ʈ ǥ��: " + text); // �ʿ� �� TMP_Text �� ����
    }
}
