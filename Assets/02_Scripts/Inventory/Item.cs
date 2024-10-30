using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id { get; private set; }
    public string name { get; private set; }
    public string explain { get; private set; }
    public string iconPath { get; private set; }
    public int price { get; private set; }
    public int count { get; private set; }

    public Item(int id, string name, string explain, string iconPath, int price, int count)
    {
        this.id = id;
        this.name = name;
        this.explain = explain;
        this.iconPath = iconPath;
        this.price = price;
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

