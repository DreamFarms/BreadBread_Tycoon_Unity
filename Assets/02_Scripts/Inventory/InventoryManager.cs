using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    //인벤토리 정보 관리
    public Dictionary<int, ItemSlot> breadInventory {get; private set;} //빵 정보
    public Dictionary<int, ItemSlot> ingredientInventory { get; private set;}//재료 정보
    [SerializeField] private InventoryUIManager inventoryUIManager;
    private GameObject inventroy;

    private void Awake()
    {
        Instance = this;
        breadInventory = new Dictionary<int, ItemSlot>();
        ingredientInventory = new Dictionary<int, ItemSlot>();
        inventroy = transform.root.gameObject;
    }

    private void Start()
    {
        inventroy.SetActive(false);
    }

    //인벤토리를 업데이트 하는 함수
    //만약 기존 정보에 없다면 새롭게 추가, 있다면 정보만 바꿈
    public void UpdateInventory(int id, Item item)
    {
        ItemSlot slot =  new ItemSlot(id, item);
        Dictionary<int, ItemSlot> targetInventory = GetTargetInventory(id);

        UpdateOrAddInventory(slot, targetInventory);
    }

    private Dictionary<int, ItemSlot> GetTargetInventory(int id)
    {
        return (id / 1000 == 1) ? breadInventory : ingredientInventory;
    }

    private void UpdateOrAddInventory(ItemSlot slot, Dictionary<int, ItemSlot> inventory)
    {
        inventory[slot.id] = slot;  // 존재하면 업데이트, 없으면 추가
        if (slot.id / 1000 == 1)
        {
            inventoryUIManager.UpdateInventoryUIData(State.Bread, slot);
        }
        else
        {
            inventoryUIManager.UpdateInventoryUIData(State.Ingredient, slot);
        }
    }

    public bool CheckIsPlateItem()
    {
        for (int i = 0; i < breadInventory.Count; i++)
        {
            if (inventoryUIManager.inventoryUI[i].isPlate == true)
            {
                print("true");
                return true;
            }
        }
        return false;
    }
}
