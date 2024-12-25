using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    //�κ��丮 ���� ����
    public Dictionary<int, ItemSlot> breadInventory {get; private set;} //�� ����
    public Dictionary<int, ItemSlot> ingredientInventory { get; private set;}//��� ����
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

    //�κ��丮�� ������Ʈ �ϴ� �Լ�
    //���� ���� ������ ���ٸ� ���Ӱ� �߰�, �ִٸ� ������ �ٲ�
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
        inventory[slot.id] = slot;  // �����ϸ� ������Ʈ, ������ �߰�
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
