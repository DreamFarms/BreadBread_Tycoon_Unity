using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI explain;
    [SerializeField] public int id = 0;

    public void UpdateBreadUI(ItemSlot itemSlot)
    {
        Item item = itemSlot.GetItemSlot(itemSlot);

        itemSprite.sprite = LoadSprite(item.iconPath);
        name.text = item.name;
        count.text = "수량: " + item.count.ToString();
        price.text = "가격: " + item.price.ToString();
        id = item.id;
    }

    public void UpdateIngredientUI(ItemSlot itemSlot)
    {
        Item item = itemSlot.GetItemSlot(itemSlot);

        itemSprite.sprite = LoadSprite(item.iconPath);
        name.text = item.name;
        count.text = "수량: " + item.count.ToString();
        id = item.id;
    }

    public void UpdateCount(int num)
    {
        count.text = "수량: " + num.ToString();
    }

    public Sprite LoadSprite(string path)
    {
        print(path);
        return Resources.Load<Sprite>(path);
    }

    public void ResetUI()
    {
        itemSprite.sprite = null;
        name.text = null;
        price.text = null;
        count.text = null;
    }
}
