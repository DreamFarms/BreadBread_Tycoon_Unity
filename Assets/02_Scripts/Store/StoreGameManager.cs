using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreGameManager : MonoBehaviour
{
    #region Instance
    private static StoreGameManager _instance;

    public static StoreGameManager Instance { get { return _instance; } }
    #endregion


    // plate Ŭ����
    private Plate plate = new Plate();

    // �޴� ������ ����Ǵ� ����
    // �̸� / ���� / ����
    public Dictionary<string, ExcelReader.Menu> dicMenu = new Dictionary<string, ExcelReader.Menu>(); // ��ǰ�� : menu(��ǰ �̸�, ����, ����)

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
        //AudioManager.Instance.PlayBGM(BGM.Store);
    }

    // ���ÿ� ������ ��ġ�ϴ� �޼���
    public void PlateSelectedMenu(string menuName)
    {
        plate.PlateSelectedMenu(menuName);
    }

    // ���� ������ dic���� �̾��ִ� �޼���
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
