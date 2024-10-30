using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public static string breadPath = @"Breads\";
    public static string ingredientPath = @"Ingredients\";

    private void Awake()
    {
        Instance = this;
    }

    private Dictionary<int, Item> itemDictionary = new Dictionary<int, Item>
    {
        {1000, new Item(1000, "초코 도넛", "초코를 바른 도넛", breadPath + "Donut_Chocolate", 800, 0) },
        {1001, new Item(1001, "딸기 아이스박스", "가수 바바(baba)가 홍보하는 아이스박스 케이크!", breadPath + "Icebox_Strawberry", 1600, 0) },
        {1002, new Item(1002, "딸기 롤케이크", "하우스 딸기가 촘촘 박힌 딸기 롤케이크!", breadPath + "RollCake_Strawberry", 2200, 0) },
        {1003, new Item(1003, "샌드위치", "바쁠때 하나씩 물고 가세요.", breadPath + "Sandwich", 1300, 0) },
        {1004, new Item(1004, "모닝빵", "아침에 먹어요 냠냠", breadPath + "DinnerRoll", 300, 0) },
        {2005, new Item(2005, "박력분", "박력적인 박력분", ingredientPath + "Flour_Red", 0, 0) }
    };

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
