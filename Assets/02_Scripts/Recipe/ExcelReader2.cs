using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//[System.Serializable]
//public class Ingredient
//{
//    public string index;
//    public string name;
//}


public class ExcelReader2 : MonoBehaviour
{
    private void Start()
    {
        ReadCSV();
    }

    private void ReadCSV()
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

            for (int i = 1; i < splitData.Length; i++) 
            {
                instance.ingredientInfoDic.Add(splitData[i], splitData[0]);
            }
        }

        instance.SetIngrredientsFromDic();



    }

}
