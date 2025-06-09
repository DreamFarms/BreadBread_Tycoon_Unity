using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    public Image backgroundImage;
    public Image itemImage;
    public TMP_Text countText;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
        countText = transform.GetChild(1).GetComponent<TMP_Text>();
    }
}
