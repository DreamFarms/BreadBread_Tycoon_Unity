using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngredientIndexInfo : MonoBehaviour
{
    [SerializeField] private List<GameObject> indexButtons;

    private void OnEnable()
    {
        string[] names = RecipeUIManager.Instance.indexNames;
        int num = 0;

        foreach(Transform child in transform)
        {
            indexButtons.Add(child.gameObject);
            child.GetChild(0).GetComponent<TMP_Text>().text = names[num];
            num++;
        }
    }
}
