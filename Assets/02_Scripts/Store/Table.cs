using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject plate, bascket;
    public GameObject food01, food02;
    public TMP_Text foodCoundText01, foodCountText02;
    public int foodCount01, foodCount02;

    private void Awake()
    {
        // 서버 or 인벤토리에서 받아온 가게의 음식 개수를 담기
    }
}
