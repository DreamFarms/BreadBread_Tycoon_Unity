using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallPositions : MonoBehaviour
{
    [SerializeField] public List<Transform> ingredientPositions = new List<Transform>();
    private int index = 0;

    private RecipeGameManager recipeGameManage;

    private void Awake()
    {
        foreach(Transform tr in ingredientPositions)
        {
            tr.gameObject.SetActive(false);
        }

    }

    private void Start()
    {
        recipeGameManage = RecipeGameManager.Instance;
    }
    public void PutIngredient(Sprite sprite)
    {
        // 이미 선택된 재료라면 list에서 삭제
        if(recipeGameManage.selectedIngredientDic.ContainsKey(sprite.name))
        {
            int index = recipeGameManage.selectedIngredientDic[sprite.name];
            ingredientPositions[index].gameObject.SetActive(false);
            recipeGameManage.selectedIngredientDic.Remove(sprite.name);
            return;
        }
        for (int i = 0; i < ingredientPositions.Count; i++)
        {
            if (!ingredientPositions[i].gameObject.activeSelf)
            {
                // 재료 배치
                ingredientPositions[i].gameObject.SetActive(true);
                ingredientPositions[i].GetComponent<SpriteRenderer>().sprite = sprite;

                // 재료 인덱스 설정
                recipeGameManage.selectedIngredientDic[sprite.name] = i;
                return;
            }
        }

        Debug.Log("모든 리스트가 다 찼습니다. 재료를 제거하고 다시 시도하세요.");
    }
}
