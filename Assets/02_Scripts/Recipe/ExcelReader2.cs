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
        public string type;
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
        TextAsset csvFile = Resources.Load<TextAsset>("Files/EnKoMapping");
        if (csvFile == null)
        {
            Debug.LogError("EnKoMapping.csv not found in Resources/Files/");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var splitData = line.Split(',');

            if (splitData.Length < 2) continue;

            char firstChar = splitData[0][0];
            if ((firstChar >= 'A' && firstChar <= 'Z') || (firstChar >= 'a' && firstChar <= 'z'))
            {
                IndexEnum index = new IndexEnum();
                index.enName = splitData[0].Trim();
                index.koName = splitData[1].Trim();

                GameManager.Instance.indexInfoDic[index.enName] = index.koName;
            }
        }
    }

    private void ReadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Files/MenuInfo");
        if (csvFile == null)
        {
            Debug.LogError("MenuInfo.csv not found in Resources/Files/");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        List<Bread> menuList = new List<Bread>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var splitData = line.Split(',');
            if (splitData.Length < 5) continue;

            Bread menu = new Bread();
            menu.enName = splitData[0].Trim();
            menu.koName = splitData[1].Trim();
            menu.price = splitData[2].Trim();
            menu.description = splitData[3].Trim();
            menu.type = splitData[4].Trim();

            GameManager.Instance.breadInfoDic[menu.enName] = menu;
        }

        Debug.Log("Complete Read File!!");
    }

    private void ReadIngredientInfoCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Files/IngredientInfo");
        if (csvFile == null)
        {
            Debug.LogError("IngredientInfo.csv not found in Resources/Files/");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var splitData = line.Split(',');
            if (splitData.Length < 3) continue;

            char firstChar = splitData[1][0];
            if ((firstChar >= 'A' && firstChar <= 'Z') || (firstChar >= 'a' && firstChar <= 'z'))
            {
                IngredientEnum ingredient = new IngredientEnum();
                ingredient.index = splitData[0].Trim();
                ingredient.enName = splitData[1].Trim();
                ingredient.koName = splitData[2].Trim();

                RecipeGameManager.Instance.IngredientInfoDic[ingredient.enName] = ingredient.koName;
            }
        }
    }
}
