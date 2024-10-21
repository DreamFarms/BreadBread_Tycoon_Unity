using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeGameManager : MonoBehaviour
{
    private static RecipeGameManager _instance;

    public static RecipeGameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Dictionary<string, string> ingredientInfoDic = new Dictionary<string, string>();

    public List<string> powders = new List<string>();
    public List<string> dairies = new List<string>();
    public List<string> processed = new List<string>();
    public List<string> fruit = new List<string>();

    public void SetIngrredientsFromDic()
    {
        foreach (var item in ingredientInfoDic)
        {
            switch(item.Value)
            {
                case "가루류":
                    powders.Add(item.Key);
                    break;
                case "유제품":
                    dairies.Add(item.Key);
                    break;
                case "과일":
                    fruit.Add(item.Key);
                    break;
                case "가공식품":
                    processed.Add(item.Key);
                    break;
            }
        }
    }

}
