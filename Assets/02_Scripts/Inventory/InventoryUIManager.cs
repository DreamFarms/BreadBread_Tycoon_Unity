using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //처음데이터 설정
    public void SetInventoryData(Dictionary<int, ItemSlot> bread, Dictionary<int, ItemSlot> ingredient)
    {
        breadUIData = bread;
        ingredientUIData = ingredient;
    }

    //인벤토리 UI Update 관리
    public void UpdateInventoryUIData(State state, ItemSlot itemSlot)
    {
        if (state == State.Bread)
        {
            if (breadUIData.TryGetValue(itemSlot.id, out ItemSlot existItemSlot))
            {
                if (inventoryState == State.Bread)
                {
                    UpdateInventoryUI(state, itemSlot);
                }
            }
            else
            {
                AddInventoryUI(state, itemSlot);
            }
            breadUIData[itemSlot.id] = itemSlot;
        }
        else if(state == State.Ingredient)
        {
            if (ingredientUIData.TryGetValue(itemSlot.id, out ItemSlot existItemSlot))
            {
                if (inventoryState == State.Ingredient)
                {
                    UpdateInventoryUI(state, itemSlot);
                }
            }
            else
            {
                AddInventoryUI(state, itemSlot);
            }
            ingredientUIData[itemSlot.id] = itemSlot;
            print(itemSlot.id);
        }
    }

    //기존에 없던 거라면 추가
    public void AddInventoryUI(State state, ItemSlot itemSlot)
    {
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
        else
        {
            return;
        }
    }

    //원래 있던거라면 UI 업데이트
    public void UpdateInventoryUI(State state, ItemSlot itemSlot)
    {
        int count = itemSlot.CheckCount(itemSlot.GetItemSlot(itemSlot));
        ItemSlotUI findItemSlotUI = inventoryUI.Find(inventoryUI => inventoryUI.id == itemSlot.id);
        findItemSlotUI.UpdateCount(count);
    }

    //내가 선택한게 빵이냐 재료냐에 따라서 상태를 변환
    //UI 전부 바꿈
    public void ChangeInventory(State state)
    {
        if (state == inventoryState)
            return;

        ResetInventoryUI();
        inventoryState = (inventoryState == State.Bread) ? State.Ingredient : State.Bread;

        if (inventoryState == State.Bread)
        {
            var sortedItems = breadUIData.OrderBy(kvp => kvp.Key).ToList();
            for (int i = 0; i < sortedItems.Count; i++)
            {
                inventoryUI[i].UpdateBreadUI(sortedItems[i].Value);
            }
        }
        else
        {
            var sortedItems = ingredientUIData.OrderBy(kvp => kvp.Key).ToList();
            for (int i = 0; i < sortedItems.Count; i++)
            {
                inventoryUI[i].UpdateIngredientUI(sortedItems[i].Value);
            }
        }
    }

    //인벤토리 UI 리셋
    public void ResetInventoryUI()
    {
        for (int i = 0; i <inventoryUI.Count; i++)
        {
            inventoryUI[i].ResetUI();
        }
    }
}
