using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ư ���� ������Ʈ�� ��ũ��Ʈ�� �߰�
// ������ ����Ǹ� �ڱ� �ڽ��� ��� Ŭ����
public class ButtonEvent : MonoBehaviour
{
    Button btn;
    private bool isClick = false;
    // ���õ� ��ư�� ����� ��
    private Color _gray = new Color(0.45f, 0.45f, 0.45f);

    private string koName;
    private string price;
    private string description;

    void Start()
    {
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(ChangeColor);
        btn.onClick.AddListener(OnClickOpenDescription);

        Button_BJH.Instance.AddMenuBtn(gameObject);
    }

    private void ChangeColor()
    {
        Button_BJH.Instance.ChangeButtonColor(gameObject);
    }

    // �ش� �޴��� Ŭ���ϸ� ������ �����
    private void OnClickOpenDescription()
    {
        if(koName == null)
        {
            List<string> list = new List<string>();
            list = GameManager.Instance.GetSelectedMenuInfo(gameObject.name);
            koName = list[0];
            price = list[1];
            description = list[2];
        }
        Debug.Log($"���õ� ���� �̸��� {koName}, ������ {price}, ������ {description}");

        // uimanager�� �ִ� menu info text�� �����ϴ� �޼���
        UIManager.Instance.SetSlectedMenuInfoText(koName, price, description);
    }

    // uimanager�� �ִ� menu info text�� �����ϴ� �޼���
    //private void EditMenuInfo()
    //{
    //    UIManager ui = UIManager.Instance;
    //    ui.menuKoName.text = this.koName;
    //    ui.menuPrice.text = this.price;
    //    ui.menuDescription.text = this.description;

        
    //}


}
