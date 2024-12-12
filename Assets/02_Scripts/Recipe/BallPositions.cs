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
        // �̹� ���õ� ����� list���� ����
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
                // ��� ��ġ
                ingredientPositions[i].gameObject.SetActive(true);
                ingredientPositions[i].GetComponent<SpriteRenderer>().sprite = sprite;

                // ��� �ε��� ����
                recipeGameManage.selectedIngredientDic[sprite.name] = i;
                return;
            }
        }

        Debug.Log("��� ����Ʈ�� �� á���ϴ�. ��Ḧ �����ϰ� �ٽ� �õ��ϼ���.");
    }
}
