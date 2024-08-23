using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ExcelReader : MonoBehaviour
{
    public string csvFileName = "menu";
    //public Dictionary<string, Menu> dicMenu = new Dictionary<string, Menu>(); // ��ǰ�� : menu(��ǰ �̸�, ����, ����)

    [System.Serializable]
    public class Menu
    {
        public string enName;
        public string koName;
        public string price; // ��������
        public string description;
    }

    private void Start()
    {
        ReadCSV();
    }

    private void ReadCSV()
    {
        // ���� ���
        string path = "Files/MenuInfo.csv";

        // �����͸� �����ϴ� ����Ʈ
        List<Menu> menuList = new List<Menu>();

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while(isFinish == false)
        {
            string data = reader.ReadLine(); // �� �� �б�
            if(data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(','); // �޸��� ������ ����
            
            Menu menu = new Menu();
            menu.enName = splitData[0];
            menu.koName = splitData[1];
            menu.price = splitData[2];
            menu.description = splitData[3];
            
            GameManager.Instance.dicMenu.Add(menu.enName, menu);
            //dicMenu.Add(menu.enName, menu);
        }
        Debug.Log("Complete Read File!!");
        Debug.Log($"{GameManager.Instance.dicMenu.Count}���� dictionary�� �߰��Ǿ����ϴ�.");
    }
}
