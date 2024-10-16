using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : MonoBehaviour
{
    [SerializeField] Button recipeBookButton; // 레시피 북 버튼
    [SerializeField] GameObject recipeBookImage; // 레시피 북 이미지

    private void OnEnable()
    {
        recipeBookImage.SetActive(false);

        recipeBookButton.onClick.AddListener(() => recipeBookImage.SetActive(!recipeBookImage.activeSelf));
    }

}
