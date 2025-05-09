using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public int Gold {  get; private set; }
    public int Exp { get; private set; }
    public string ItemName { get; private set; }
    public int ItemCount { get; private set; }

    public Reward(int gold =  0, int exp = 0, string itemName = null, int itemCount = 0)
    {
        this.Gold = gold;
        this.Exp = exp;
        this.ItemName = itemName;
        this.ItemCount = itemCount;
    }


}
