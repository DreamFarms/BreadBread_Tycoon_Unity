using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class IndexEnum
{
    public string enName;
    public string koName;
}


[System.Serializable]
public class IngredientEnum
{
    public string index;
    public string enName;
    public string koName;
}


public class ExcelReader2 : MonoBehaviour
{
    [System.Serializable]
    public class Bread
    {
        public string enName;
        public string koName;
        public string price; // ㅇㅇㅇ원
        public string description;
    }
    // 해당 리더는 리팩토링이 필요합니다.
    // 모든 리더는 하나로 통합하고, gamemanager에 데이터를 저장하는 방향으로 추후 수정 해야합니다.

    private void Start()
    {
        ReadCSV();
        ReadIndexInfoCSV();
        ReadIngredientInfoCSV();
    }

    private void ReadIndexInfoCSV()
    {
        // 파일 경로
        string path = "Files/EnKoMapping.csv";

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while (isFinish == false)
        {
            string data = reader.ReadLine(); // 한 줄 읽기
            if (data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(','); // 콤마로 데이터 분할

            char firstChar = splitData[0][0];
            if (firstChar >= 'A' && firstChar <= 'Z' || firstChar >= 'a' && firstChar <= 'z')
            {
                IndexEnum index = new IndexEnum();
                index.enName = splitData[0].Trim();
                index.koName = splitData[1].Trim();

                GameManager.Instance.indexInfoDic[index.enName] = index.koName;
            }

        }
    }

    // 해당 메서드는 excelreader에서 가져온 코드
    // 임시로 긁어온 것이니 추후에 병합 필요
    private void ReadCSV()
    {
        // 파일 경로
        string path = "Files/MenuInfo.csv";

        // 데이터를 저장하는 리스트
        List<Bread> menuList = new List<Bread>();

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while (isFinish == false)
        {
            string data = reader.ReadLine(); // 한 줄 읽기
            if (data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(','); // 콤마로 데이터 분할

            Bread menu = new Bread();
            menu.enName = splitData[0];
            menu.koName = splitData[1];
            menu.price = splitData[2];
            menu.description = splitData[3];

            GameManager.Instance.breadInfoDic.Add(menu.enName, menu);
            //dicMenu.Add(menu.enName, menu);
        }
        Debug.Log("Complete Read File!!");
    }


    private void ReadIngredientInfoCSV()
    {
        // 파일 경로
        string path = "Files/IngredientInfo.csv";

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while (isFinish == false)
        {
            string data = reader.ReadLine(); // 한 줄 읽기
            if (data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(','); // 콤마로 데이터 분할

            char firstChar = splitData[1][0];
            if (firstChar >= 'A' && firstChar <= 'Z' || firstChar >= 'a' && firstChar <= 'z')
            {
                IngredientEnum ingredient = new IngredientEnum();
                ingredient.index = splitData[0];
                ingredient.enName = splitData[1];
                ingredient.koName = splitData[2];

                RecipeGameManager.Instance.IngredientInfoDic.Add(ingredient.enName, ingredient.koName);
            }

        }
    }

}
