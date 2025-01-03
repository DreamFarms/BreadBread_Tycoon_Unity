using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreGameManager : MonoBehaviour
{
    #region Instance
    private static StoreGameManager _instance;

    public static StoreGameManager Instance { get { return _instance; } }
    #endregion


    // plate 클래스
    private Plate plate = new Plate();

    // 메뉴 정보가 저장되는 변수
    // 이름 / 가격 / 설명
    public Dictionary<string, ExcelReader.Menu> dicMenu = new Dictionary<string, ExcelReader.Menu>(); // 상품명 : menu(상품 이름, 가격, 정보)

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        plate = new Plate();
        //AudioManager.Instance.PlayBGM(BGM.Store);
    }

    // 접시에 음식을 배치하는 메서드
    public void PlateSelectedMenu(string menuName)
    {
        if (menuName == null)
            return;

        plate.PlateSelectedMenu(menuName);
    }

    public void ReturnSelectedMenu()
    {
        plate.ReturnSelectMenu();
    }

    // 음식 정보를 dic에서 뽑아주는 메서드
    public List<string> GetSelectedMenuInfo(string menuEnName)
    {
        List<string> menuInfoList = new List<string>();
        ExcelReader.Menu menu = new ExcelReader.Menu();
        menu = dicMenu[menuEnName];
        menuInfoList.Add(menu.koName);
        menuInfoList.Add(menu.price);
        menuInfoList.Add(menu.description);

        return menuInfoList;
    }

    public void SelectedBakeBread()
    {

    }
}
