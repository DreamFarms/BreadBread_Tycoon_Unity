using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ExcelReader3 : MonoBehaviour
{
    public static string breadPath = @"Breads\";
    public static string ingredientPath = @"Ingredients\";
    [SerializeField] InventoryConnection inventoryConnection;

    public Dictionary<string, string> enKoMapping = new Dictionary<string, string>();
    public Dictionary<string, string> ingerdientMapping = new Dictionary<string, string>();

    private void Start()
    {
        ReadIndexInfoCSV();
        ReadIngredientInfoCSV();
        ReadKJY_MenuInfoCSV();
        ReadKJY_IngredientInfoCSV();
        inventoryConnection.StartInventoryConnection();
    }
    /*
        private void ReadKJY_IngredientInfoCSV()
        {
            //���� ���
            string path = "Files/KJY_IngredientInfo.csv";

            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

            ItemManager instance = ItemManager.Instance;

            bool isFinish = false;

            while (isFinish == false)
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

            while (isFinish == false)
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
                // �̹���, �޴��̸�_�ѱ�, ����, �޴�����, ���, ���̵�(int), ����
                Item item = new Item(breadPath + tempList[i], tempList[i + 1], tempList[i + 2], tempList[i + 3], tempList[i + 4], int.Parse(tempList[i + 5]), 0);
                ItemManager.Instance.SetItemDictionary(item.id, item);
            }
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
                    IndexEnum index = new IndexEnum();
                    index.enName = splitData[0].Trim();
                    index.koName = splitData[1].Trim();

                    //enKoMapping[index.enName] = index.koName;
                    InfoManager.Instance.SetEnKoInfoDic(index.enName, index.koName);
                }

            }
        }

        private void ReadIngredientInfoCSV()
        {
            // ���� ���
            string path = "Files/IngredientInfo.csv";

            // stream reader
            StreamReader reader = new StreamReader(Application.streamingAssetsPath + "/" + path);

            // instance
            RecipeGameManager instance = RecipeGameManager.Instance;

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

                    //ingerdientMapping[ingredient.enName] = ingredient.koName;
                    InfoManager.Instance.SetIngredientInfoDic(ingredient.enName, ingredient.koName);
                }

            }
        }
    */
    private void ReadKJY_IngredientInfoCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Files/KJY_IngredientInfo");
        if (csvFile == null)
        {
            Debug.LogError("KJY_IngredientInfo.csv ������ ã�� �� �����ϴ�.");
            return;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string data;
            while ((data = reader.ReadLine()) != null)
            {
                var splitData = data.Split(',');

                if (splitData.Length < 4) continue;

                char firstChar = splitData[1][0];
                if ((firstChar >= 'A' && firstChar <= 'Z') || (firstChar >= 'a' && firstChar <= 'z'))
                {
                    Item item = new Item(
                        ingredientPath + splitData[1],
                        splitData[2],
                        "0",
                        splitData[2],
                        null,
                        int.Parse(splitData[3]),
                        0
                    );
                    Debug.Log(item);
                    ItemManager.Instance.SetItemDictionary(item.id, item);
                }
            }
        }
    }

    private void ReadKJY_MenuInfoCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Files/KJY_MenuInfo");
        if (csvFile == null)
        {
            Debug.LogError("KJY_MenuInfo.csv ������ ã�� �� �����ϴ�.");
            return;
        }

        List<string> tempList = new List<string>();
        int idx = 0;

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string data;
            while ((data = reader.ReadLine()) != null)
            {
                foreach (Match match in Regex.Matches(data, "\"([^\"]*)\""))
                {
                    if (idx >= 6)
                    {
                        tempList.Add(match.Groups[1].Value);
                    }
                    idx++;
                }
            }
        }

        for (int i = 0; i < tempList.Count; i += 6)
        {
            Item item = new Item(
                breadPath + tempList[i],
                tempList[i + 1],
                tempList[i + 2],
                tempList[i + 3],
                tempList[i + 4],
                int.Parse(tempList[i + 5]),
                0
            );
            Debug.Log(item);
            ItemManager.Instance.SetItemDictionary(item.id, item);
        }
    }

    private void ReadIndexInfoCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Files/EnKoMapping");
        if (csvFile == null)
        {
            Debug.LogError("EnKoMapping.csv ������ ã�� �� �����ϴ�.");
            return;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string data;
            while ((data = reader.ReadLine()) != null)
            {
                var splitData = data.Split(',');
                if (splitData.Length < 2) continue;

                char firstChar = splitData[0][0];
                if ((firstChar >= 'A' && firstChar <= 'Z') || (firstChar >= 'a' && firstChar <= 'z'))
                {
                    IndexEnum index = new IndexEnum();
                    index.enName = splitData[0].Trim();
                    index.koName = splitData[1].Trim();
                    Debug.Log(index.enName);
                    InfoManager.Instance.SetEnKoInfoDic(index.enName, index.koName);
                }
            }
        }
    }

    private void ReadIngredientInfoCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Files/IngredientInfo");
        if (csvFile == null)
        {
            Debug.LogError("IngredientInfo.csv ������ ã�� �� �����ϴ�.");
            return;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string data;
            while ((data = reader.ReadLine()) != null)
            {
                var splitData = data.Split(',');
                if (splitData.Length < 3) continue;

                char firstChar = splitData[1][0];
                if ((firstChar >= 'A' && firstChar <= 'Z') || (firstChar >= 'a' && firstChar <= 'z'))
                {
                    IngredientEnum ingredient = new IngredientEnum();
                    ingredient.index = splitData[0].Trim();
                    ingredient.enName = splitData[1].Trim();
                    ingredient.koName = splitData[2].Trim();
                    Debug.Log(ingredient.enName);
                    InfoManager.Instance.SetIngredientInfoDic(ingredient.enName, ingredient.koName);
                }
            }
        }
    }
}
