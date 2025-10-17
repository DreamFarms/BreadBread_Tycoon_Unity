using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlSpawner : MonoBehaviour
{
    [Header("Spawn Points (3개 지정)")]
    [SerializeField] private RectTransform[] spawnPoints;   // UI일 때
    // 만약 3D 오브젝트로 쓰면 RectTransform → Transform 으로 바꿔주세요

    [SerializeField] private GameObject sausagePrefab;

    private readonly List<SausageItem> current = new();
    private int takenSinceLastRefill = 0;

    private void Start()
    {
        SpawnAll();
    }

    public void NotifyTaken(SausageItem item)
    {
        // 현재 리스트에서 제거
        current.RemoveAll(x => x == null);
        current.Remove(item);

        takenSinceLastRefill++;

        // 3개 모두 가져가면 한 번에 리필
        if (takenSinceLastRefill >= spawnPoints.Length)
        {
            takenSinceLastRefill = 0;
            SpawnAll();
        }
    }

    public void NotifyConsumed(SausageItem item)
    {
        current.RemoveAll(x => x == null);
        current.Remove(item);
    }

    private void SpawnAll()
    {
        foreach (var pt in spawnPoints)
        {
            var go = Instantiate(sausagePrefab, pt);
            var rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero; // 포인트 중앙에 붙임
            rect.localScale = Vector3.one;

            var sausage = go.GetComponent<SausageItem>();
            sausage.OwnerSpawner = this;
            current.Add(sausage);
        }
    }
}
