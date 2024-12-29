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

        currentCategory = category01; // 기본으로 보여줄 카테고리는 무조건 01번

        
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
            page.recipeInfo.text = "아직 발견한 레시피가 없습니다.";
            basicPage.SetActive(true);
        }
        else
        {
            // todo
            // 찾은 레시피가 존재하면 어떤 행위를 할 것인지 추가
            foreach (string findedRecipeName in findedRecipes)
            {
                // page를 카테고리로 편입
                string type = GameManager.Instance.breadInfoDic[findedRecipeName].type;
                BreadType enumType = (BreadType)Enum.Parse(typeof(BreadType), type);
                switch(enumType)
                {
                    case BreadType.bread:
                        GameObject page1 = Instantiate(recipeBookPageGo, category01.transform);

                        // page 설정
                        // 중복되는 코드 정리 필요
                        RecipeBookPage recipeBookPage1 = page1.GetComponent<RecipeBookPage>();
                        recipeBookPage1.recipeImage.gameObject.SetActive(true);
                        recipeBookPage1.SetRecipeBookPage(findedRecipeName);

                        category01.GetComponent<Category>().AddPages(recipeBookPage1);
                        category01.GetComponent<Category>().InitPage();

                        break;

                    case BreadType.desert:
                        GameObject page2 = Instantiate(recipeBookPageGo, category02.transform);
                        // page 설정
                        RecipeBookPage recipeBookPage2 = page2.GetComponent<RecipeBookPage>();
                        recipeBookPage2.recipeImage.gameObject.SetActive(true);
                        recipeBookPage2.SetRecipeBookPage(findedRecipeName);

                        category02.GetComponent<Category>().AddPages(recipeBookPage2);
                        category02.GetComponent<Category>().InitPage();

                        break;

                    case BreadType.special:
                        GameObject page3 = Instantiate(recipeBookPageGo, category03.transform);
                        // page 설정
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
        // page를 카테고리로 편입
        string type = GameManager.Instance.breadInfoDic[findedRecipeName].type;
        BreadType enumType = (BreadType)Enum.Parse(typeof(BreadType), type);
        switch (enumType)
        {
            case BreadType.bread:
                GameObject page1 = Instantiate(recipeBookPageGo, category01.transform);

                // page 설정
                // 중복되는 코드 정리 필요
                RecipeBookPage recipeBookPage1 = page1.GetComponent<RecipeBookPage>();
                recipeBookPage1.recipeImage.gameObject.SetActive(true);
                recipeBookPage1.SetRecipeBookPage(findedRecipeName);

                category01.GetComponent<Category>().AddPages(recipeBookPage1);
                category01.GetComponent<Category>().InitPage();

                break;

            case BreadType.desert:
                GameObject page2 = Instantiate(recipeBookPageGo, category02.transform);
                // page 설정
                RecipeBookPage recipeBookPage2 = page2.GetComponent<RecipeBookPage>();
                recipeBookPage2.recipeImage.gameObject.SetActive(true);
                recipeBookPage2.SetRecipeBookPage(findedRecipeName);

                category02.GetComponent<Category>().AddPages(recipeBookPage2);
                category02.GetComponent<Category>().InitPage();

                break;

            case BreadType.special:
                GameObject page3 = Instantiate(recipeBookPageGo, category03.transform);
                // page 설정
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
