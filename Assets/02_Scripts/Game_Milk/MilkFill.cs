using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkFill : MonoBehaviour
{
    [SerializeField] private GameObject[] risingImages;
    
    private int currnetIndex;
    private bool isComplete = false;

    public bool IsComplete
    {
        get { return isComplete; }
    }

    private void OnEnable()
    {
        foreach(GameObject go in risingImages)
        {
            go.SetActive(false);
        }

        int target = MakeRandomNumber();
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
        currnetIndex++;

        // 실패
        if(currnetIndex >= risingImages.Length - 1)
        {
            risingImages[risingImages.Length - 1].SetActive(true);
            isComplete = false;
        }
        // 성공
        else
        {
            risingImages[currnetIndex].SetActive(true);
            isComplete = false;
            if(currnetIndex == risingImages.Length - 2)
            {
                isComplete = true;
            }
        }
    }
}
