using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    public IngredientEvent ingredientEvent;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            ingredientEvent = new IngredientEvent();
        }
    }
}
