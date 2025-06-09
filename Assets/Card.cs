using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cardRenderer;

    [SerializeField]
    private SpriteRenderer ingredientRenderer;

    [SerializeField]
    private Sprite backgroundSprite; // assign

    [SerializeField]
    private Sprite frontSprite; // 재료.. board.cs에서 적용

    [SerializeField]
    private Sprite backSprite; // 뒷면

    private bool isFilpped; // true : 앞면 false : 뒷면
    private bool isFilpping;
    private bool isMatched;

    public int cardId;

    public void SetCardId(int id)
    {
        this.cardId = id;
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public void SetIngredientSprite(Sprite sprite)
    {
        this.frontSprite = sprite;
    }

    public void FlipCard()
    {

        isFilpping = true;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0f, originalScale.y, originalScale.z);

        transform.DOScale(targetScale, 0.2f).OnComplete(() =>
        {
            isFilpped = !isFilpped;

            if (isFilpped)
            {
                cardRenderer.sprite = backgroundSprite;
                ingredientRenderer.sprite = frontSprite;
            }
            else
            {
                cardRenderer.sprite = null;
                ingredientRenderer.sprite = backSprite;
            }

            transform.DOScale(originalScale, 0.2f).OnComplete(() =>
            {
                isFilpping = false;
            });
        });

    }

    private void OnMouseDown()
    {
        if (!isFilpping && !isMatched && !isFilpped) // 뒤집히는 중도 아니고 매치 판정 중도 아니며 뒤집힌 상태도 아닐때만 뒤집기 진행
        {
            CardGameManager.Instance.CardClicked(this); // 내가 클릭되면 나를 뒤집어 주는 것은 card game manager
        }
    }
}
