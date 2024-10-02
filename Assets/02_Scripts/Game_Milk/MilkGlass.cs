using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkGlass : MonoBehaviour
{
    [SerializeField] private GameObject[] risingImages;

    private void OnEnable()
    {
        foreach(GameObject go in risingImages)
        {
            go.SetActive(false);
        }

        int target = MakeRandomNumber();
        risingImages[target].SetActive(true);
    }

    private int MakeRandomNumber()
    {
        int max = risingImages.Length-1;
        int random = UnityEngine.Random.Range(0, max);
        return random;
    }
}
