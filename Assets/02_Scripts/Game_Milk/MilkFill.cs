using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkFill : MonoBehaviour
{
    [SerializeField] private GameObject[] risingImages;
    
    private int currnetIndex;

    public bool GetResult()
    {
        Debug.Log(gameObject.name);
        Debug.Log(currnetIndex);
        if (currnetIndex == risingImages.Length - 2)
        {
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        foreach(GameObject go in risingImages)
        {
            go.SetActive(false);
        }

        int target = MakeRandomNumber(); // 0 ~ 2까지
        risingImages[target].SetActive(true);
        currnetIndex = target;
    }

    private int MakeRandomNumber()
    {
        int max = risingImages.Length-1;
        int random = UnityEngine.Random.Range(0, max);
        return random;
    }

    public void FillMilk()
    {
        ++currnetIndex;

        // 실패
        if(currnetIndex >= risingImages.Length - 1)
        {
            risingImages[risingImages.Length - 1].SetActive(true);
        }
        // 성공
        else
        {
            risingImages[currnetIndex].SetActive(true);
        }
    }
}
