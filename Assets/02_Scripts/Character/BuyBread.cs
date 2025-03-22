using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBread : MonoBehaviour
{
    private NPCCharacter npc;
    int num = 0;

    private void Start()
    {
        npc = GetComponent<NPCCharacter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Table")
        {
            npc.SetCharacterPrefer(StoreGameManager.Instance.platelist[num].menuName);
            num++;
        }
    }
}
