using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string iconPath { get; private set; }
    public string name { get; private set; }
    public string price { get; private set; }
    public string explain { get; private set; }
    public string[] breadIngredients { get; private set; }
    public int id { get; private set; }
    public int count { get; private set; }

    public Item(string iconPath, string name, string price, string explain, string breadIngredients,int id, int count)
    {
        this.iconPath = iconPath;
        this.name = name;
        this.price = price;
        this.explain = explain;
        this.breadIngredients = SetIngredeients(breadIngredients);
        this.id = id;
        this.count = count;
    }

    public string[] SetIngredeients(string ingredients)
    {
        if (ingredients == null)
        {
            return null;
        }

        var split = ingredients.Split(',');
        breadIngredients = new string[split.Length];
        
        if (split.Length == 1)
        {
            breadIngredients[0] = ingredients;
            return breadIngredients;
        }

        for (int i = 0; i < split.Length; i++)
        {
            breadIngredients[i] = split[i];
        }
        return breadIngredients;
    }

    public void IncreaseCount(int num)
    {
        count += num;
    }

    public void DecreaseCount(int num)
    {
        count -= num;
        if (count < 0)
        {
            count = 0;
        }
    }
}

