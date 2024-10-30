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
        {1000, new Item(1000, "���� ����", "���ڸ� �ٸ� ����", breadPath + "Donut_Chocolate", 800, 0) },
        {1001, new Item(1001, "���� ���̽��ڽ�", "���� �ٹ�(baba)�� ȫ���ϴ� ���̽��ڽ� ����ũ!", breadPath + "Icebox_Strawberry", 1600, 0) },
        {1002, new Item(1002, "���� ������ũ", "�Ͽ콺 ���Ⱑ ���� ���� ���� ������ũ!", breadPath + "RollCake_Strawberry", 2200, 0) },
        {1003, new Item(1003, "������ġ", "�ٻܶ� �ϳ��� ���� ������.", breadPath + "Sandwich", 1300, 0) },
        {1004, new Item(1004, "��׻�", "��ħ�� �Ծ�� �ȳ�", breadPath + "DinnerRoll", 300, 0) },
        {2005, new Item(2005, "�ڷº�", "�ڷ����� �ڷº�", ingredientPath + "Flour_Red", 0, 0) }
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
