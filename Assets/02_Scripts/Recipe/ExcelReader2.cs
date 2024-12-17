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
    // 해당 리더는 리팩토링이 필요합니다.
    // 모든 리더는 하나로 통합하고, gamemanager에 데이터를 저장하는 방향으로 추후 수정 해야합니다.

    private void Start()
    {
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
                Index index = new Index();
                index.enName = splitData[0].Trim();
                index.koName = splitData[1].Trim();


                InfoManager.Instance.enKoMappingDic[index.enName] = index.koName;
                RecipeGameManager.Instance.IndexInfoDic[index.enName] = index.koName;
            }

        }
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
                Ingredient ingredient = new Ingredient();
                ingredient.index = splitData[0];
                ingredient.enName = splitData[1];
                ingredient.koName = splitData[2];

                RecipeGameManager.Instance.IngredientInfoDic.Add(ingredient.enName, ingredient.koName);
            }

        }
    }

}
