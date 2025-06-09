using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private Sprite[] cardSprites;

    private List<int> cardIdList = new List<int>();
    private List<Card> cardList = new List<Card>();

    void Start()
    {
        GenerateCardId();
        ShuffleCardId();
        InitBoard();
    }


    private void GenerateCardId()
    {
        for (int i = 0; i < cardSprites.Length; i++)
        {
            // 0 0 1 1 2 2 3 3 4 4 ...
            cardIdList.Add(i);
            cardIdList.Add(i);
        }
    }
    void ShuffleCardId()
    {
        for (int i = 0; i < cardIdList.Count; i++)
        {
            int randomIdx = UnityEngine.Random.Range(i, cardIdList.Count);
            int temp = cardIdList[randomIdx];
            cardIdList[randomIdx] = cardIdList[i];
            cardIdList[i] = temp;
        }
    }

    void InitBoard()
    {
        float x = 1.28f;
        float y = 1.75f;

        int rowCount = 5;
        int columnCount = 4;

        int cardIdx = 0;

        for (float row = -0.2f; row < rowCount-0.2f; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                float posX = (col - (int)(columnCount / 2)) * x + (x / 2); 
                float posY = (row - (int)(rowCount / 2)) * y;
                Vector3 pos = new Vector3(posX, posY, 0f);
                GameObject go = Instantiate(cardPrefab, pos, Quaternion.identity);
                Card card = go.GetComponent<Card>();
                int cardId = cardIdList[cardIdx++];
                card.SetCardId(cardId);
                card.SetIngredientSprite(cardSprites[cardId]);
                
                cardList.Add(card);
            }
        }
    }

    public List<Card> GetCard()
    {
        return cardList;
    }
}
