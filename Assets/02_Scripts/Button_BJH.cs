using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 버튼을 관리하고
// 버튼 관련된 상호작용을 관리하는 클래스
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

    // 메뉴 선택 버튼을 관리하는 리스트
    private List<GameObject> _menuBtnList = new List<GameObject>();
    // 이전에 선택한 메뉴 버튼을 저장하는 변수
    public GameObject recentMenuBtn;


    // 메뉴 버튼 리스트에 값을 추가하는 메서드
    // 각 버튼에서 해당 메서드를 호출하여 자기 자신을 등록
    public void AddMenuBtn(GameObject GameObject)
    {
        _menuBtnList.Add(GameObject);
    }

    // 직전 선택한 버튼의 색을 변경하고 선택된 음식을 저장하는 메서드
    // 색을 변경할 버튼 게임오브젝트를 매개변수로 받음
    public void ChangeButtonColor(GameObject clickedBtn)
    {
        // 이미 클릭된 상태에서 한번 더 클릭했을 때
        if(recentMenuBtn == clickedBtn)
        {
            clickedBtn.GetComponent<Image>().color = Color.white;
            recentMenuBtn = null;
            return;
        }

        // 이전 클릭된 버튼이 없을 때
        if(recentMenuBtn == null)
        {
            clickedBtn.GetComponent<Image>().color = Color.gray;
            recentMenuBtn = clickedBtn;
        }
        // 이전 클릭된 버튼이 있을 때
        else
        {
            recentMenuBtn.GetComponent<Image>().color = Color.white;
            clickedBtn.GetComponent<Image>().color = Color.gray;
            recentMenuBtn = clickedBtn;
        }
    }

    // 메뉴를 선택하고 Select! 버튼을 누르면 작동하는 메서드
    // 수량 체크 UI가 뜨고, 선택한 메뉴의 이미지가 보임
    public void OnOpenMenuCountingImg()
    {
        if (recentMenuBtn == null) {  return; }
        UIManager um = UIManager.Instance;
        um.OnInactiveAllUI(); // 이전에 켜져있던 UI 끄기
        um.OnActiveThisUI(um.selectMenuCountingImg);
        um.selectedMenuImg.GetComponent<Image>().sprite = recentMenuBtn.GetComponent<Image>().sprite;
    }

    public void OnPlateSelectedMenu()
    {
        GameManager.Instance.PlateSelectedMenu(recentMenuBtn.gameObject.name);
        UIManager.Instance.EnableBG(false); // 백그라운드 끄기
    }

}
