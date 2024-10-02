using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMilkGlass : MonoBehaviour
{
    [SerializeField] private string destroyTargetName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains(destroyTargetName))
        {
            Destroy(collision.gameObject);
        }
    }
}
