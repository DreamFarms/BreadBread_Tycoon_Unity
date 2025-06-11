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
    private Sprite frontSprite; // ���.. board.cs���� ����

    [SerializeField]
    private Sprite backSprite; // �޸�

    private bool isFilpped; // true : �ո� false : �޸�
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
        if (!isFilpping && !isMatched && !isFilpped) // �������� �ߵ� �ƴϰ� ��ġ ���� �ߵ� �ƴϸ� ������ ���µ� �ƴҶ��� ������ ����
        {
            CardGameManager.Instance.CardClicked(this); // ���� Ŭ���Ǹ� ���� ������ �ִ� ���� card game manager
        }
    }
}
