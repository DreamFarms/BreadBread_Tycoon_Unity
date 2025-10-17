using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlSpawner : MonoBehaviour
{
    [Header("Spawn Points (3�� ����)")]
    [SerializeField] private RectTransform[] spawnPoints;   // UI�� ��
    // ���� 3D ������Ʈ�� ���� RectTransform �� Transform ���� �ٲ��ּ���

    [SerializeField] private GameObject sausagePrefab;

    private readonly List<SausageItem> current = new();
    private int takenSinceLastRefill = 0;

    private void Start()
    {
        SpawnAll();
    }

    public void NotifyTaken(SausageItem item)
    {
        // ���� ����Ʈ���� ����
        current.RemoveAll(x => x == null);
        current.Remove(item);

        takenSinceLastRefill++;

        // 3�� ��� �������� �� ���� ����
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
            rect.anchoredPosition = Vector2.zero; // ����Ʈ �߾ӿ� ����
            rect.localScale = Vector3.one;

            var sausage = go.GetComponent<SausageItem>();
            sausage.OwnerSpawner = this;
            current.Add(sausage);
        }
    }
}
