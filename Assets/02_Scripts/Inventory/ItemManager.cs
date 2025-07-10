using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetItemDictionary(int id, Item item)
    {
        itemDictionary.Add(id,item);
    }

    public Dictionary<int, Item> ItemData
    {
        get { return itemDictionary; }
        private set { itemDictionary = value; }
    }

    public void PlusItemCount(int id, int num)
    {
        if (itemDictionary.TryGetValue(id, out Item item))
        {
            item.IncreaseCount(num);
            print(item.name);
            InventoryManager.Instance.UpdateInventory(id, item);
        }
        else
        {
            return;
        }
    }

    public void MinusItemCount(int id, int num)
    {
        if (itemDictionary.TryGetValue(id, out Item item))
        {
            item.DecreaseCount(num);
            InventoryManager.Instance.UpdateInventory(id, item);
        }
        else
        {
            return;
        }
    }
}
