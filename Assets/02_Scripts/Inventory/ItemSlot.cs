using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot
{
    public int id;
    private Item item;

    public ItemSlot(int id, Item item)
    {
        this.id = id;
        this.item = item;
    }

    public int CheckCount(Item item)
    {
        return item.count;
    }

    public Item GetItemSlot(ItemSlot slot)
    {
        return slot.item;
    }
}
