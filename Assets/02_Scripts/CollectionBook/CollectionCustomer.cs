using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CollectionCustomer : MonoBehaviour
{
    public string customerName { get; private set; } = string.Empty;
    public string customerIntro { get; private set; } = string.Empty;
    public List<string> preferredBreads { get; private set; } = new List<string>();
    public bool preferredBreadsVisible { get; private set; } = false;
    public int visitCount { get; private set; } = 0;
    public bool unlocked { get; private set; } = false;
    public Sprite customerSprite { get; private set; } = null;

    [SerializeField] private Image customerImage;
    [SerializeField] private CollectionPopUp popUp;
    [SerializeField] private CollectionBookManager manager;

    private void Start()
    {
        manager = FindAnyObjectByType<CollectionBookManager>();
        popUp = manager.popUp.GetComponent<CollectionPopUp>();
    }

    public void SetCustomerInfo(string name, string intro, List<string> pb, bool pbv, int count, bool unlocked, Sprite sprite)
    {
        customerName = name;
        customerIntro = intro;
        preferredBreads = pb;
        preferredBreadsVisible = pbv;
        visitCount = count;
        this.unlocked = unlocked;
        customerImage.sprite = sprite;
        customerSprite = sprite;
    }

    public void PopUp()
    {
        manager.popUp.SetActive(true);
        if (unlocked)
        {
            popUp.SetUpPopUp(customerName, customerIntro, preferredBreads, preferredBreadsVisible, visitCount, unlocked, customerImage.sprite);
        }
        else
        {
            popUp.SetUpPopUp(string.Empty, string.Empty, null, false, 0, false, customerImage.sprite);
        }
    }


}
