using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 버튼 게임 오브젝트에 스크립트로 추가
// 게임이 실행되면 자기 자신을 담는 클래스
public class ButtonEvent : MonoBehaviour
{
    Button btn;
    private bool isClick = false;
    // 선택된 버튼이 변경될 색
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

    // 해당 메뉴를 클릭하면 설명이 노출됨
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
        Debug.Log($"선택된 빵의 이름은 {koName}, 가격은 {price}, 설명은 {description}");

        // uimanager에 있는 menu info text를 수정하는 메서드
        UIManager.Instance.SetSlectedMenuInfoText(koName, price, description);
    }

    // uimanager에 있는 menu info text를 수정하는 메서드
    //private void EditMenuInfo()
    //{
    //    UIManager ui = UIManager.Instance;
    //    ui.menuKoName.text = this.koName;
    //    ui.menuPrice.text = this.price;
    //    ui.menuDescription.text = this.description;

        
    //}


}
