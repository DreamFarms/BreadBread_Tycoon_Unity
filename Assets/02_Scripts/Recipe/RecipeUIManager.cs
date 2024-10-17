using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : MonoBehaviour
{
    [SerializeField] private Button recipeBookButton; // ·¹½ÃÇÇ ºÏ ¹öÆ°
    [SerializeField] private GameObject recipeBookImage; // ·¹½ÃÇÇ ºÏ ÀÌ¹ÌÁö
    [SerializeField] private Button recipeBookCloseBtn; // ·¹½ÃÇÇ ºÏ ´Ý±â ¹öÆ°

    [SerializeField] private GameManager ball; // ¹Í½Ìº¼

    private void OnEnable()
    {
        recipeBookImage.SetActive(false);

        recipeBookButton.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookImage);
        });

        recipeBookCloseBtn.onClick.AddListener(() =>
        {
            ActiveGameobject(recipeBookButton.gameObject);
            ActiveGameobject(recipeBookImage);
        });


    }

    private void ActiveGameobject(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }
}
