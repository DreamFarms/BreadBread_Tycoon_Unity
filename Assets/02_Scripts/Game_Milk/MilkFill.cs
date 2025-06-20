using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MilkFill : MonoBehaviour
{
    [SerializeField] private SpriteRenderer milkBottle; // target assign
    [SerializeField] private Sprite[] milkBottleSprits; // 0 : 멀쩡한 병   1 : 깨진 병

    [SerializeField] private SpriteRenderer milkFillSprite; // target assign
    [SerializeField] private GameObject[] milkFillGameobjects; // sprits
    
    private int currnetIndex;

    // ??
    public bool GetResult()
    {
        if (currnetIndex == milkFillGameobjects.Length - 2)
        {
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        InitMilkeBottle();
    }

    private void InitMilkeBottle()
    {
        milkBottle.sprite = milkBottleSprits[0];


        foreach(GameObject go in milkFillGameobjects)
        {
            go.SetActive(false);
        }

        int target = MakeRandomNumber();
        milkFillGameobjects[target].SetActive(true);
        currnetIndex = target;
        
    }

    private int MakeRandomNumber()
    {
        int max = milkFillGameobjects.Length-1;
        int random = UnityEngine.Random.Range(0, max);
        return random;
    }

    public void FillMilk()
    {
        ++currnetIndex;

        // 실패
        if (currnetIndex >= milkFillGameobjects.Length - 1)
        {
            milkBottle.sprite = milkBottleSprits[1]; // bottle
            milkFillGameobjects[milkFillGameobjects.Length - 1].SetActive(true); // milk
        }
        // 성공
        else
        {
            milkFillGameobjects[currnetIndex].SetActive(true);
        }
    }
}
