using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum State
{
    Bread,
    Ingredient
}

public class InventoryUIManager : MonoBehaviour
{
    public  List<ItemSlotUI> inventoryUI;
    private Dictionary<int, ItemSlot> breadUIData;
    private Dictionary<int, ItemSlot> ingredientUIData;

    public Button breadBtn;
    public Button ingredientBtn;

    public State inventoryState;
    [SerializeField] private GameObject content;

    private void Awake()
    {
        inventoryState = State.Bread;
        breadUIData = new Dictionary<int, ItemSlot>();
        ingredientUIData = new Dictionary<int, ItemSlot>();
    }

    private void OnEnable()
    {
        breadBtn.onClick.AddListener(() => ChangeInventory(State.Bread));
        ingredientBtn.onClick.AddListener(() => ChangeInventory(State.Ingredient));
    }

    //ó�������� ����
    public void SetInventoryData(Dictionary<int, ItemSlot> bread, Dictionary<int, ItemSlot> ingredient)
    {
        breadUIData = bread;
        ingredientUIData = ingredient;
    }

    //�κ��丮 UI Update ����
    public void UpdateInventoryUIData(State state, ItemSlot itemSlot)
    {
        if (state == State.Bread)
        {
            if (breadUIData.TryGetValue(itemSlot.id, out ItemSlot existItemSlot))
            {
                UpdateInventoryUI(state, itemSlot);
            }
            else
            {
                AddInventoryUI(state, itemSlot);
            }
            breadUIData[itemSlot.id] = itemSlot;
        }
        else
        {
            if (breadUIData.TryGetValue(itemSlot.id, out ItemSlot existItemSlot))
            {
                UpdateInventoryUI(state, itemSlot);
            }
            else
            {
                AddInventoryUI(state, itemSlot);
            }
            ingredientUIData[itemSlot.id] = itemSlot;
        }
    }

    //������ ���� �Ŷ�� �߰�
    public void AddInventoryUI(State state, ItemSlot itemSlot)
    {
        ItemSlotUI ui = new ItemSlotUI();

        if (state == State.Bread && inventoryState == State.Bread)
        {
            for (int i = 0; i < inventoryUI.Count; i++)
            {
                if (inventoryUI[i].id == 0)
                {
                    inventoryUI[i].UpdateBreadUI(itemSlot);
                    return;
                }
            }
        }
        else if (state == State.Ingredient && inventoryState == State.Ingredient)
        {
            for (int i = 0; i < inventoryUI.Count; i++)
            {
                if (inventoryUI[i].id == 0)
                {
                    inventoryUI[i].UpdateIngredientUI(itemSlot);
                    return;
                }
            }
        }

    }

    //���� �ִ��Ŷ�� UI ������Ʈ
    public void UpdateInventoryUI(State state, ItemSlot itemSlot)
    {
        int count = itemSlot.CheckCount(itemSlot.GetItemSlot(itemSlot));
        ItemSlotUI findItemSlotUI = inventoryUI.Find(inventoryUI => inventoryUI.id == itemSlot.id);
        findItemSlotUI.UpdateCount(count);
    }

    //���� �����Ѱ� ���̳� ���Ŀ� ���� ���¸� ��ȯ
    //UI ���� �ٲ�
    public void ChangeInventory(State state)
    {
        if (state == inventoryState)
            return;
        
        ResetInventoryUI();
        inventoryState = (inventoryState == State.Bread) ? State.Ingredient : State.Bread;

        if (inventoryState == State.Bread)
        {
            for (int i = 0; i < breadUIData.Count; i++)
            {
                inventoryUI[i].UpdateBreadUI(breadUIData[i]);
            }
        }
        else
        {
            for (int i = 0; i < ingredientUIData.Count; i++)
            {
                inventoryUI[i].UpdateIngredientUI(ingredientUIData[i]);
            }
        }
    }

    //�κ��丮 UI ����
    public void ResetInventoryUI()
    {
        for (int i = 0; i <inventoryUI.Count; i++)
        {
            inventoryUI[i].ResetUI();
        }
    }
}
