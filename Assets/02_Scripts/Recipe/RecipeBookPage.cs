using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookPage : MonoBehaviour
{
    public Image recipeImage;
    public TMP_Text recipeName, recipeInfo;

    public void SetRecipeBookPage(string breadName)
    {
        recipeImage.sprite = Resources.Load<Sprite>("Breads/" + breadName);
        recipeName.text = GameManager.Instance.breadInfoDic[breadName].koName;
        recipeInfo.text = GameManager.Instance.breadInfoDic[breadName].description;
    }
}
