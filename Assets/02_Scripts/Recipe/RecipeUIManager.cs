using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIManager : MonoBehaviour
{
    [SerializeField] private Button recipeBookButton; // ������ �� ��ư
    [SerializeField] private GameObject recipeBookImage; // ������ �� �̹���
    [SerializeField] private Button recipeBookCloseBtn; // ������ �� �ݱ� ��ư

    [SerializeField] private GameManager ball; // �ͽ̺�

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
