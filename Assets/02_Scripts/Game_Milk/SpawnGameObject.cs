using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGameObject : MonoBehaviour
{
    // ½ºÆù
    [SerializeField] private GameObject milkGlassPrefab;
    [SerializeField] private float spawnPositionX; // 13.0f

    public void SpawnMilkGlass()
    {
        Vector2 spawnPosition = new Vector2();
        spawnPosition.x = spawnPositionX;
        GameObject go = Instantiate(milkGlassPrefab, transform);
        go.transform.position = spawnPosition;
    }
}
