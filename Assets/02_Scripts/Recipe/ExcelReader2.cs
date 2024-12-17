using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class Index
{
    public string enName;
    public string koName;
}


[System.Serializable]
public class Ingredient
{
    public string index;
    public string enName;
    public string koName;
}


public class ExcelReader2 : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        public string enName;
        public string koName;
        public string price; // ��������
        public string description;
    }
    // �ش� ������ �����丵�� �ʿ��մϴ�.
    // ��� ������ �ϳ��� �����ϰ�, gamemanager�� �����͸� �����ϴ� �������� ���� ���� �ؾ��մϴ�.

    private void Start()
    {
        ReadCSV();
        ReadIndexInfoCSV();
        ReadIngredientInfoCSV();
    }

    private void ReadIndexInfoCSV()
    {
        // ���� ���
        string path = "Files/EnKoMapping.csv";

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while (isFinish == false)
        {
            string data = reader.ReadLine(); // �� �� �б�
            if (data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(','); // �޸��� ������ ����

            char firstChar = splitData[0][0];
            if (firstChar >= 'A' && firstChar <= 'Z' || firstChar >= 'a' && firstChar <= 'z')
            {
                Index index = new Index();
                index.enName = splitData[0].Trim();
                index.koName = splitData[1].Trim();


                InfoManager.Instance.enKoMappingDic[index.enName] = index.koName;
                RecipeGameManager.Instance.IndexInfoDic[index.enName] = index.koName;
            }

        }
    }

    // �ش� �޼���� excelreader���� ������ �ڵ�
    // �ӽ÷� �ܾ�� ���̴� ���Ŀ� ���� �ʿ�
    private void ReadCSV()
    {
        // ���� ���
        string path = "Files/MenuInfo.csv";

        // �����͸� �����ϴ� ����Ʈ
        List<Menu> menuList = new List<Menu>();

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while (isFinish == false)
        {
            string data = reader.ReadLine(); // �� �� �б�
            if (data == null)
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

            GameManager.Instance.menuInfoDic.Add(menu.enName, menu);
            //dicMenu.Add(menu.enName, menu);
        }
        Debug.Log("Complete Read File!!");
    }


    private void ReadIngredientInfoCSV()
    {
        // ���� ���
        string path = "Files/IngredientInfo.csv";

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        bool isFinish = false;

        while (isFinish == false)
        {
            string data = reader.ReadLine(); // �� �� �б�
            if (data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(','); // �޸��� ������ ����

            char firstChar = splitData[1][0];
            if (firstChar >= 'A' && firstChar <= 'Z' || firstChar >= 'a' && firstChar <= 'z')
            {
                Ingredient ingredient = new Ingredient();
                ingredient.index = splitData[0];
                ingredient.enName = splitData[1];
                ingredient.koName = splitData[2];

                RecipeGameManager.Instance.IngredientInfoDic.Add(ingredient.enName, ingredient.koName);
            }

        }
    }

}
