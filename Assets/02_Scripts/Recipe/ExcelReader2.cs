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
        public string price; // ��������
        public string description;
        public string type;
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
        string path = "CSV_Files/EnKoMapping";

        TextAsset csvFile = Resources.Load<TextAsset>(path);

        if (csvFile == null)
        {
            Debug.LogWarning("������ ���ӿ��� EnKoMapping.csv�� ã�� �� �����ϴ�.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        foreach(string line in lines)
        {
            string[] values = line.Split(' '); // Powder,�����


        }

        

        //while (isFinish == false)
        //{
        //    string data = reader.ReadLine(); // �� �� �б�
        //    if (data == null)
        //    {
        //        isFinish = true;
        //        break;
        //    }
        //    var splitData = data.Split(','); // �޸��� ������ ����

        //    char firstChar = splitData[0][0];
        //    if (firstChar >= 'A' && firstChar <= 'Z' || firstChar >= 'a' && firstChar <= 'z')
        //    {
        //        IndexEnum index = new IndexEnum();
        //        index.enName = splitData[0].Trim();
        //        index.koName = splitData[1].Trim();

        //        GameManager.Instance.indexInfoDic[index.enName] = index.koName;
        //    }

        //}
    }

    // �ش� �޼���� excelreader���� ������ �ڵ�
    // �ӽ÷� �ܾ�� ���̴� ���Ŀ� ���� �ʿ�
    private void ReadCSV()
    {
        // ���� ���
        string path = "Files/MenuInfo.csv";

        // �����͸� �����ϴ� ����Ʈ
        List<Bread> menuList = new List<Bread>();

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

            Bread menu = new Bread();
            menu.enName = splitData[0]; // sandwich
            menu.koName = splitData[1]; // ������ġ
            menu.price = splitData[2]; // 500
            menu.description = splitData[3]; // ���¸� ������ �Ѷ��Ѷ� ���� ������ġ�Դϴ�.
            menu.type = splitData[4]; // bread

            GameManager.Instance.breadInfoDic.Add(menu.enName, menu);
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
                IngredientEnum ingredient = new IngredientEnum();
                ingredient.index = splitData[0];
                ingredient.enName = splitData[1];
                ingredient.koName = splitData[2];

                RecipeGameManager.Instance.IngredientInfoDic.Add(ingredient.enName, ingredient.koName);
            }

        }
    }

}
