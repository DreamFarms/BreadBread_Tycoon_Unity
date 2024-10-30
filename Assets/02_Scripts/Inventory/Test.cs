using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    int i = 0;
    int j = 0;
    Dictionary<int, Item> test;

    private void Start()
    {
        test = ItemManager.Instance.ItemData;
    }

    private void Update()
    {
        if (i == 6)
        {
            i = 0;
        }
        
        if (j == 6)
        {
            j = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ItemManager.Instance.PlusItemCount(test.ElementAt(i).Key, 1);
            i++;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ItemManager.Instance.MinusItemCount(test.ElementAt(j).Key, 1);
            j++;
        }
    }
}
