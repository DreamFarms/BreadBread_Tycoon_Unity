using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookImage : MonoBehaviour
{
    public Button closeButton;

    public GameObject recipeBookPageGo; // prefab
    public GameObject basicPage; // go
    public Button categoryBtn01, categoryBtn02, categoryBtn03; // assign
    public GameObject category01, category02, category03; // assign
    public GameObject currentCategory;
    public List<string> findedRecipes = new List<string>();

    private void Awake()
    {
        categoryBtn01.onClick.AddListener(() => OnClickCategoryBtn(category01));
        categoryBtn02.onClick.AddListener(() => OnClickCategoryBtn(category02));
        categoryBtn03.onClick.AddListener(() => OnClickCategoryBtn(category03));


        category01.SetActive(true);
        category02.SetActive(false);
        category03.SetActive(false);

        currentCategory = category01; // �⺻���� ������ ī�װ��� ������ 01��

        
    }

    public void OnClickCategoryBtn(GameObject categoryGo)
    {
        currentCategory.SetActive(false);
        categoryGo.SetActive(true);
        currentCategory = categoryGo;
    }

    public void InitRecipeBook()
    {
        findedRecipes = RecipeGameManager.Instance.findedRecipes;

        if (findedRecipes.Count == 0)
        {
            RecipeBookPage page = basicPage.GetComponent<RecipeBookPage>();
            page.recipeImage.gameObject.SetActive(false);
            page.recipeName.text = null;
            page.recipeInfo.text = "���� �߰��� �����ǰ� �����ϴ�.";
            basicPage.SetActive(true);
        }
        else
        {
            // todo
            // ã�� �����ǰ� �����ϸ� � ������ �� ������ �߰�
            foreach (string findedRecipeName in findedRecipes)
            {
                // page�� ī�װ��� ����
                string type = GameManager.Instance.breadInfoDic[findedRecipeName].type;
                BreadType enumType = (BreadType)Enum.Parse(typeof(BreadType), type);
                switch(enumType)
                {
                    case BreadType.bread:
                        GameObject page1 = Instantiate(recipeBookPageGo, category01.transform);

                        // page ����
                        // �ߺ��Ǵ� �ڵ� ���� �ʿ�
                        RecipeBookPage recipeBookPage1 = page1.GetComponent<RecipeBookPage>();
                        recipeBookPage1.recipeImage.gameObject.SetActive(true);
                        recipeBookPage1.SetRecipeBookPage(findedRecipeName);

                        category01.GetComponent<Category>().AddPages(recipeBookPage1);
                        category01.GetComponent<Category>().InitPage();

                        break;

                    case BreadType.desert:
                        GameObject page2 = Instantiate(recipeBookPageGo, category02.transform);
                        // page ����
                        RecipeBookPage recipeBookPage2 = page2.GetComponent<RecipeBookPage>();
                        recipeBookPage2.recipeImage.gameObject.SetActive(true);
                        recipeBookPage2.SetRecipeBookPage(findedRecipeName);

                        category02.GetComponent<Category>().AddPages(recipeBookPage2);
                        category02.GetComponent<Category>().InitPage();

                        break;

                    case BreadType.special:
                        GameObject page3 = Instantiate(recipeBookPageGo, category03.transform);
                        // page ����
                        RecipeBookPage recipeBookPage3 = page3.GetComponent<RecipeBookPage>();
                        recipeBookPage3.recipeImage.gameObject.SetActive(true);
                        recipeBookPage3.SetRecipeBookPage(findedRecipeName);

                        category03.GetComponent<Category>().AddPages(recipeBookPage3);
                        category02.GetComponent<Category>().InitPage();

                        break;
                }
            }

        }
    }

    public void AddRecipeOnRecipeBook(string findedRecipeName)
    {
        // page�� ī�װ��� ����
        string type = GameManager.Instance.breadInfoDic[findedRecipeName].type;
        BreadType enumType = (BreadType)Enum.Parse(typeof(BreadType), type);
        switch (enumType)
        {
            case BreadType.bread:
                GameObject page1 = Instantiate(recipeBookPageGo, category01.transform);

                // page ����
                // �ߺ��Ǵ� �ڵ� ���� �ʿ�
                RecipeBookPage recipeBookPage1 = page1.GetComponent<RecipeBookPage>();
                recipeBookPage1.recipeImage.gameObject.SetActive(true);
                recipeBookPage1.SetRecipeBookPage(findedRecipeName);

                category01.GetComponent<Category>().AddPages(recipeBookPage1);
                category01.GetComponent<Category>().InitPage();

                break;

            case BreadType.desert:
                GameObject page2 = Instantiate(recipeBookPageGo, category02.transform);
                // page ����
                RecipeBookPage recipeBookPage2 = page2.GetComponent<RecipeBookPage>();
                recipeBookPage2.recipeImage.gameObject.SetActive(true);
                recipeBookPage2.SetRecipeBookPage(findedRecipeName);

                category02.GetComponent<Category>().AddPages(recipeBookPage2);
                category02.GetComponent<Category>().InitPage();

                break;

            case BreadType.special:
                GameObject page3 = Instantiate(recipeBookPageGo, category03.transform);
                // page ����
                RecipeBookPage recipeBookPage3 = page3.GetComponent<RecipeBookPage>();
                recipeBookPage3.recipeImage.gameObject.SetActive(true);
                recipeBookPage3.SetRecipeBookPage(findedRecipeName);

                category03.GetComponent<Category>().AddPages(recipeBookPage3);
                category03.GetComponent<Category>().InitPage();

                break;
        }
    }


    public void OnClickNextPageButton()
    {
        Category category = currentCategory.GetComponent<Category>();
        category.NextPage();
    }
}
