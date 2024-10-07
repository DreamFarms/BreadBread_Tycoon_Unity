using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkFill : MonoBehaviour
{
    [SerializeField] private GameObject[] risingImages;
    private int currnetIndex;

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

        if(currnetIndex >= risingImages.Length - 1)
        {
            risingImages[risingImages.Length - 1].SetActive(true);
        }
        else
        {
            risingImages[currnetIndex].SetActive(true);
        }
    }
}
