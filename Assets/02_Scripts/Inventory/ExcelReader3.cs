using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class ExcelReader3 : MonoBehaviour
{
    public static string breadPath = @"Breads\";
    public static string ingredientPath = @"Ingredients\";
    [SerializeField] InventoryConnection inventoryConnection;

    private void Awake()
    {
        ItemManager instance = ItemManager.Instance;
        ReadIndexInfoCSV();
        ReadIngredientInfoCSV();
        ReadKJY_MenuInfoCSV();
        ReadKJY_IngredientInfoCSV();
        inventoryConnection.StartInventoryConnection();
    }

    private void ReadKJY_IngredientInfoCSV()
    {
        //파일 경로
        string path = "Files/KJY_IngredientInfo.csv";

        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        ItemManager instance = ItemManager.Instance;

        bool isFinish = false;

        while(isFinish == false)
        {
            string data = reader.ReadLine();
            if (data == null)
            {
                isFinish = true;
                break;
            }
            var splitData = data.Split(',');

            char firstChar = splitData[1][0];
            if (firstChar >= 'A' && firstChar <= 'Z' || firstChar >= 'a' && firstChar <= 'z')
            {
                Item item = new Item(ingredientPath + splitData[1], splitData[2].ToString(), "0", splitData[2], null, int.Parse(splitData[3]), 0);
                ItemManager.Instance.SetItemDictionary(item.id, item);
            }
        }
    }

    private void ReadKJY_MenuInfoCSV()
    {
        string path = "Files/KJY_MenuInfo.csv";

        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        ItemManager instance = ItemManager.Instance;

        List<string> tempList = new List<string>();

        bool isFinish = false;
        int idx = 0;

        while(isFinish == false)
        {
            string data = reader.ReadLine();
            if (data == null)
            {
                isFinish = true;
                break;
            }

            foreach (Match match in Regex.Matches(data, "\"([^\"]*)\""))
            {
                if (idx >= 6)
                {
                    tempList.Add(match.Groups[1].Value);
                }
                idx++;
            }
        }

        for (int i = 0; i < tempList.Count; i += 6)
        {
            Item item = new Item(breadPath + tempList[i], tempList[i + 1], tempList[i + 2], tempList[i + 3], tempList[i + 4], int.Parse(tempList[i + 5]), 0);
            ItemManager.Instance.SetItemDictionary(item.id, item);
        }
    }

    private void ReadIndexInfoCSV()
    {
        // 파일 경로
        string path = "Files/EnKoMapping.csv";

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        // instance
        RecipeGameManager instance = RecipeGameManager.Instance;

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
                Index index = new Index();
                index.enName = splitData[0].Trim();
                index.koName = splitData[1].Trim();


                InfoManager.Instance.enKoMappingDic[index.enName] = index.koName;
                Debug.Log(InfoManager.Instance.enKoMappingDic[index.enName]);
                //InfoManager.Instance.enKoMappingDic.Add(index.koName, index.enName);
                //RecipeGameManager.Instance.IndexInfoDic[index.enName] = index.koName;
            }

        }
    }

    private void ReadIngredientInfoCSV()
    {
        // 파일 경로
        string path = "Files/IngredientInfo.csv";

        // stream reader
        StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

        // instance
        RecipeGameManager instance = RecipeGameManager.Instance;

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
                Ingredient ingredient = new Ingredient();
                ingredient.index = splitData[0];
                ingredient.enName = splitData[1];
                ingredient.koName = splitData[2];

                InfoManager.Instance.SetIngredientInfoDic(ingredient.enName, ingredient.koName);
            }

        }
    }
}
