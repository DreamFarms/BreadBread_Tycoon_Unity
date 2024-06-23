using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPickerGameManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> railPositions = new List<Transform>();

    private List<Sprite> sprites = new List<Sprite>();

    private void Update()
    {
        // 만약 z, x를 누르면 레일 위치 변경
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyUp(KeyCode.X))
        {
            MoveFruitToFront();
        }
    }

    // 레일 위 과일 순서를 변경하는 메서드
    // 한 칸 앞으로 이동
    private void MoveFruitToFront()
    {
        GetSprites();

        for (int i = 0; i < railPositions.Count; i++)
        {
            if (sprites[i] == null)
            {
                continue;
            }
            railPositions[i].GetComponent<SpriteRenderer>().sprite = sprites[i+1];
        }
    }

    private void GetSprites()
    {
        sprites.Clear();
        foreach(var rail in railPositions)
        {
            var sprite = rail.GetComponent<SpriteRenderer>().sprite;
            sprites.Add(sprite);
        }
    }
}
