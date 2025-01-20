using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 도넛 접시 버튼을 클릭하면
// Clciked UI가 생성되는 테스트
// Clicked UI에서 Plate를 누르면 해당하는 메뉴가 접시 위로 스폰됨
public class Plate : MonoBehaviour
{
    // 선택된 접시 정보 저장
    [SerializeField] private GameObject _selectedPlate;
    public bool isPlate {get; private set; }
    public string menuName { get; private set; }

    private void Start()
    {
        isPlate = false;
        menuName = string.Empty;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ClickPlateTest();
        }
    }

    // 접시를 클릭하면, 메뉴 선택 UI가 뜨는 메서드
    private void ClickPlateTest()
    {
        // 마우스 위치
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);

        // raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePos2, Vector2.zero);

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.CompareTag("Plate"))
            {
                _selectedPlate = hit.collider.gameObject;
                UIManager um = UIManager.Instance;
                um.OnActiveThisUI(um.selectPlateMenuImg);
                um.OnActiveThisUI(um.breadInfoImgGroup);
            }
        }
    }

    // 메뉴를 선택하면 선택된 메뉴가 진열되고
    // UI에 "선택됨" 표시가 나타나는 메서드
    // 버튼 스스로를 GameObject를 매개변수로 담는다.
    public void OnclickMenuBtn(GameObject go)
    {
        go.GetComponent<Image>().color = new Color(0.45f, 0.45f, 0.45f);
    }

    // 선택된 접시에 메뉴를 진열하는 메서드
    public void PlateSelectedMenu(string menuName, GameObject plate)
    {
        print("inPlateHere");
        Sprite sprite = Resources.Load<Sprite>("Breads/" + menuName);
        // 이슈 : _selectedPlate에서 null이 발생. 분명 Plate 게임오브젝트가 할당 된 상태
        // 해결이 안되서 find로 찾기 시작. 하지만 성능 문제가 있으므로 곧 해결 해야됨
        GameObject childGo = plate.transform.GetChild(0).gameObject;
        SpriteRenderer spriteRenderer = childGo.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            GameObject go = new GameObject("Food");
            Instantiate(go, _selectedPlate.transform);
            _selectedPlate.AddComponent<SpriteRenderer>();
        }
        // 음식 올라가는 부분 레이어를 3으로 설정
        spriteRenderer.sortingOrder = 3;
        spriteRenderer.sprite = sprite;
        isPlate = true;
        this.menuName = menuName;
        print("inPlateHere");
    }

    public void ReturnSelectMenu(GameObject plate)
    {
        GameObject childGo = plate.transform.GetChild(0).gameObject;
        SpriteRenderer spriteRenderer = childGo.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
        isPlate = false;
    }

    

}
