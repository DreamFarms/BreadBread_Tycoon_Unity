using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInfo : MonoBehaviour
{
    private Sprite[] powders;
    private Sprite[] dairies;
    private void Awake()
    {
        powders = Resources.LoadAll<Sprite>("Ingredients/Powder"); // �����
        dairies = Resources.LoadAll<Sprite>("Ingredients/Dairy"); // ����ǰ
    }
}
