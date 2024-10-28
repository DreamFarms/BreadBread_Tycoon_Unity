using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    [Header("Connection")]
    [SerializeField] private string _url = "";

    // plate Ŭ����
    private Plate plate = new Plate();


    // �޴� ������ ����Ǵ� ����
    // �̸� / ���� / ����
    public Dictionary<string, ExcelReader.Menu> dicMenu = new Dictionary<string, ExcelReader.Menu>(); // ��ǰ�� : menu(��ǰ �̸�, ����, ����)

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.Store);
    }


    public string Url
    {
        get { return _url; }
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
