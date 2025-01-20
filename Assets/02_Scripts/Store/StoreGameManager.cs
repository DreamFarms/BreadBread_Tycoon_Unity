using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreGameManager : MonoBehaviour
{
    #region Instance
    private static StoreGameManager _instance;

    public static StoreGameManager Instance { get { return _instance; } }
    #endregion


    // plate 클래스
    //private List<Plate> platelist;
    public List<Plate> platelist;
    public List<GameObject> tableList;
    private int count;

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
        //plate = new Plate();
        //AudioManager.Instance.PlayBGM(BGM.Store);
        count = FindObjectsOfType<Table>().Length;
        platelist = new List<Plate>(count);
        tableList = GetFirstChildObjects();
    }

    public List<GameObject> GetFirstChildObjects()
    {
        List<GameObject> firstChildObjects = new List<GameObject>();

        Table[] tableObjects = FindObjectsOfType<Table>();

        foreach (Table table in tableObjects)
        {
            Transform parentTransform = table.gameObject.transform;

            if (parentTransform.childCount > 0)
            {
                Transform firstChild = parentTransform.GetChild(0);
                firstChildObjects.Add(firstChild.gameObject);

                Debug.Log($"Parent: {parentTransform.name}, First Child: {firstChild.name}");
            }
        }

        return firstChildObjects;
    }

    // 접시에 음식을 배치하는 메서드
    public void PlateSelectedMenu(string menuName)
    {
        if (menuName == null)
            return;
        for (int i = 0; i < count; i++)
        {
            if (i >= platelist.Count)
            {
                var newPlate = new Plate();
                platelist.Add(newPlate);
                newPlate.PlateSelectedMenu(menuName, tableList[i]);
                break; // 리스트에 새로 추가했으므로 반복문 종료
            }

            if (platelist[i].isPlate == true)
            {
                continue;
            }
            else
            {
                platelist[i].PlateSelectedMenu(menuName, tableList[i]);
                break;
            }
        }
    }

    public void ReturnSelectedMenu(string menuName)
    {
        for (int i = 0; i < platelist.Count; i++)
        {
            if (platelist[i].menuName == menuName)
            {
                platelist[i].ReturnSelectMenu(tableList[i]);
            }
        }
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

    public bool CheckIsPlateFull()
    {
        if (platelist.Count < count)
        {
            return false;
        }

        for (int i = 0; i < platelist.Count; i++)
        {
            if (platelist[i].isPlate == false)
            {
                return false;
            }
        }
        return true;
    }

    public void SelectedBakeBread()
    {

    }
}
