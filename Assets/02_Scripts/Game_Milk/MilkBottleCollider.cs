using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottleCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("MilkBottle"))
        {
            MilkGameManager.Instance.MilkFill = collision.gameObject.GetComponent<MilkFill>();
        }
    }
}
