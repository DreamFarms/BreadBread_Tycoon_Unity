using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : MonoBehaviour
{
    [SerializeField] private Button recipeBookButton; // ·¹½ÃÇÇ ºÏ ¹öÆ°
    [SerializeField] private GameObject recipeBookImage; // ·¹½ÃÇÇ ºÏ ÀÌ¹ÌÁö
    [SerializeField] private GameManager ball; // ¹Í½Ìº¼

    private void OnEnable()
    {
        recipeBookImage.SetActive(false);

        recipeBookButton.onClick.AddListener(() => recipeBookImage.SetActive(!recipeBookImage.activeSelf));
    }

}
