using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewordUI : MonoBehaviour
{
    public GameObject uiGo; // assign
    public TMP_Text title;
    public Image rewordItemImage; // fail은 없음
    public TMP_Text rewordItemName; // fail은 설명
    public Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            RecipeGameManager.Instance.StopDoughAnimation();
        });

        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
