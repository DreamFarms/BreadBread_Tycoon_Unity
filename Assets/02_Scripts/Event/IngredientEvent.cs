using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientEvent : MonoBehaviour
{
    public event Action onMilkCollected;
    public void MilkCollected()
    {
        if(onMilkCollected != null)
        {
            Debug.Log("MilkCollected() 메서드를 실행");

            onMilkCollected();
        }
    }
}
