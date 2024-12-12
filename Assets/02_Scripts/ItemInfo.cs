using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public Image rewordItemImage;
    public TMP_Text rewordItemName;

    private void OnEnable()
    {
        Button button = GetComponent<Button>();
        Sprite sprite = GetComponent<Image>().sprite;
    }
}
