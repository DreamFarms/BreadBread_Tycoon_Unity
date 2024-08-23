using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ExcelReader : MonoBehaviour
{
    public string csvFileName = "menu";
    //public Dictionary<string, Menu> dicMenu = new Dictionary<string, Menu>(); // 상품명 : menu(상품 이름, 가격, 정보)

    [System.Serializable]
    public class Menu
    {
        public string enName;
        public string koName;
        public string price; // ㅇㅇㅇ원
        public string description;
    }

    private void Start()
    {
        ReadCSV();
    }

    private void ReadCSV()
    {
        // 파일 경로
        string path = "Files/MenuInfo.csv";

        // 데이터를 저장하는 리스트
        List<Menu> menuList = new List<Menu>();

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while(isFinish == false)
        {
            string data = reader.ReadLine(); // 한 줄 읽기
            if(data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(','); // 콤마로 데이터 분할
            
            Menu menu = new Menu();
            menu.enName = splitData[0];
            menu.koName = splitData[1];
            menu.price = splitData[2];
            menu.description = splitData[3];
            
            GameManager.Instance.dicMenu.Add(menu.enName, menu);
            //dicMenu.Add(menu.enName, menu);
        }
        Debug.Log("Complete Read File!!");
        Debug.Log($"{GameManager.Instance.dicMenu.Count}개가 dictionary에 추가되었습니다.");
    }
}
