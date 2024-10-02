using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDeleteMilkGlass : MonoBehaviour
{
    // 스폰
    [SerializeField] private GameObject milkGlassPrefab;
    [SerializeField] private float spawnPositionX; // 13.0f

    private void Update()
    {
        // 임시
        if(Input.GetMouseButton(0))
        {
            SpawnMilkGlass();
        }
    }

    public void SpawnMilkGlass()
    {
        Vector2 spawnPosition = new Vector2();
        spawnPosition.x = spawnPositionX;
        GameObject go = Instantiate(milkGlassPrefab, transform);
        go.transform.position = spawnPosition;
    }
}
