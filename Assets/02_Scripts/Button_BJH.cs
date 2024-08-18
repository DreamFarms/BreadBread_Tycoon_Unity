using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ư�� �����ϰ�
// ��ư ���õ� ��ȣ�ۿ��� �����ϴ� Ŭ����
public class Button_BJH : MonoBehaviour
{
    private static Button_BJH _instance;
    public static Button_BJH Instance
    { get { return _instance; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    // �޴� ���� ��ư�� �����ϴ� ����Ʈ
    private List<GameObject> _menuBtnList = new List<GameObject>();
    // ������ ������ �޴� ��ư�� �����ϴ� ����
    public GameObject recentMenuBtn;


    // �޴� ��ư ����Ʈ�� ���� �߰��ϴ� �޼���
    // �� ��ư���� �ش� �޼��带 ȣ���Ͽ� �ڱ� �ڽ��� ���
    public void AddMenuBtn(GameObject GameObject)
    {
        _menuBtnList.Add(GameObject);
    }

    // ���� ������ ��ư�� ���� �����ϰ� ���õ� ������ �����ϴ� �޼���
    // ���� ������ ��ư ���ӿ�����Ʈ�� �Ű������� ����
    public void ChangeButtonColor(GameObject clickedBtn)
    {
        // �̹� Ŭ���� ���¿��� �ѹ� �� Ŭ������ ��
        if(recentMenuBtn == clickedBtn)
        {
            clickedBtn.GetComponent<Image>().color = Color.white;
            recentMenuBtn = null;
            return;
        }

        // ���� Ŭ���� ��ư�� ���� ��
        if(recentMenuBtn == null)
        {
            clickedBtn.GetComponent<Image>().color = Color.gray;
            recentMenuBtn = clickedBtn;
        }
        // ���� Ŭ���� ��ư�� ���� ��
        else
        {
            recentMenuBtn.GetComponent<Image>().color = Color.white;
            clickedBtn.GetComponent<Image>().color = Color.gray;
            recentMenuBtn = clickedBtn;
        }
    }

    // �޴��� �����ϰ� Select! ��ư�� ������ �۵��ϴ� �޼���
    // ���� üũ UI�� �߰�, ������ �޴��� �̹����� ����
    public void OnOpenMenuCountingImg()
    {
        if (recentMenuBtn == null) {  return; }
        UIManager um = UIManager.Instance;
        um.OnInactiveAllUI(); // ������ �����ִ� UI ����
        um.OnActiveThisUI(um.selectMenuCountingImg);
        um.selectedMenuImg.GetComponent<Image>().sprite = recentMenuBtn.GetComponent<Image>().sprite;
    }

    public void OnPlateSelectedMenu()
    {
        GameManager.Instance.PlateSelectedMenu(recentMenuBtn.gameObject.name);
        UIManager.Instance.EnableBG(false); // ��׶��� ����
    }

}
