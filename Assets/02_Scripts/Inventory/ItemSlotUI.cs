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
    [SerializeField] public int id;
    [SerializeField] private Button plateButton;
    [SerializeField] private Button returnButton;
    [SerializeField] private GameObject plateBtnObj;
    [SerializeField] private GameObject returnBtnObj;
    [SerializeField] private GameObject noticePlate;

    public bool isPlate = false;

    private void Start()
    { 
        plateBtnObj.SetActive(false);
        returnBtnObj.SetActive(false);
    }

    private void OnEnable()
    {
        plateButton.onClick.RemoveAllListeners(); 
        returnButton.onClick.RemoveAllListeners();

        plateButton.onClick.AddListener(() => StoreGameManager.Instance.PlateSelectedMenu(FindEnKoMappingDIc(true)));
        returnButton.onClick.AddListener(() => StoreGameManager.Instance.ReturnSelectedMenu(FindEnKoMappingDIc(false)));
        //returnButton.onClick.AddListener(() => ChangeButton());
    }

    private string FindEnKoMappingDIc(bool value)
    {
        if (StoreGameManager.Instance.CheckIsPlateFull() == true && value == true)
        {
            noticePlate.SetActive(true);
            return null;
        }

        var dictionary = InfoManager.Instance.enKoMappingDic;
        foreach (var pair in dictionary)
        {
            if (pair.Value == name.text) // 값 비교
            {
                Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
                ChangeButton();
                return pair.Key; // 키 반환
            }
        }
        Debug.Log("없습니다");
        return null;
    }

    public void ChangeButton()
    {
        if (isPlate == false)
        {
            isPlate = true;
            plateBtnObj.SetActive(false);
        }
        else
        {
            isPlate = false;
            plateBtnObj.SetActive(true);
        }
    }

    public void UpdateBreadUI(ItemSlot itemSlot)
    {
        Item item = itemSlot.GetItemSlot(itemSlot);

        itemSprite.sprite = LoadSprite(item.iconPath);
        id = item.id;
        name.text = item.name;
        count.text = "수량: " + item.count.ToString();
        price.text = "가격: " + item.price.ToString();

        plateButton.interactable = true;

        if (isPlate == false)
        {
            plateBtnObj.SetActive(true);
        }
        returnBtnObj.SetActive(true);
    }

    public void UpdateIngredientUI(ItemSlot itemSlot)
    {
        Item item = itemSlot.GetItemSlot(itemSlot);

        itemSprite.sprite = LoadSprite(item.iconPath);
        id = item.id;
        name.text = item.name;
        count.text = "수량: " + item.count.ToString();
        plateBtnObj.SetActive(false);
        returnBtnObj.SetActive(false);
    }

    public void UpdateCount(int num)
    {
        count.text = "수량: " + num.ToString();
    }

    public Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }

    public void ResetUI()
    {
        itemSprite.sprite = null;
        name.text = null;
        price.text = null;
        count.text = null;
        id = 0;
    }
}
