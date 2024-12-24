using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    public Image rewordItemImage;
    public TMP_Text rewordItemName;
    public TMP_Text rewordItemCount;

    private void OnEnable()
    {
        // ??
        Button button = GetComponent<Button>();
        Sprite sprite = GetComponent<Image>().sprite;
    }

    public void MinusItemCount()
    {
        if (string.IsNullOrEmpty(rewordItemCount.text))
        {
            Debug.LogError("Text is null or empty!");
            return;
        }

        if(int.TryParse(rewordItemCount.text, out int count))
        {
            count--;

            if (count < 0)
            {
                Debug.LogError("재료가 없어서 배치, 감소가 안돼요");
                return;
            }

            rewordItemCount.text = count.ToString();
        }
        else
        {
            Debug.LogError($"Invalid number format: {rewordItemCount.text}");
        }
    }

    public void PlusItemCount()
    {
        if (string.IsNullOrEmpty(rewordItemCount.text))
        {
            Debug.LogError("Text is null or empty!");
            return;
        }

        if (int.TryParse(rewordItemCount.text, out int count))
        {
            count++;
            rewordItemCount.text = count.ToString();
        }
        else
        {
            Debug.LogError($"Invalid number format: {rewordItemCount.text}");
        }
    }




}
