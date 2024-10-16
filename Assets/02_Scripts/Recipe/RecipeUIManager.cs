using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : MonoBehaviour
{
    [SerializeField] Button recipeBookButton; // ������ �� ��ư
    [SerializeField] GameObject recipeBookImage; // ������ �� �̹���

    private void OnEnable()
    {
        recipeBookImage.SetActive(false);

        recipeBookButton.onClick.AddListener(() => recipeBookImage.SetActive(!recipeBookImage.activeSelf));
    }

}
