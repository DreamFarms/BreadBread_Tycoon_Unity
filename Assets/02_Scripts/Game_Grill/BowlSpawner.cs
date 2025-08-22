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

    private void Start()
    {
        SpawnAll();
    }

    public void NotifyConsumed(SausageItem item)
    {
        current.Remove(item);
        // ���� �Һ�Ǹ� �ٽ� 3�� ä���
        if (current.Count == 0) SpawnAll();
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
