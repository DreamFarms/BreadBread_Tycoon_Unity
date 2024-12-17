using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewordUI : MonoBehaviour
{
    public Image rewordItemImage;
    public TMP_Text rewordItemName;
    public Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
