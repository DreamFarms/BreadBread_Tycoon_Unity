using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubmitZone : MonoBehaviour, IDropHandler
{
    [Header("Stack UI")]
    [SerializeField] private RectTransform stackParent;
    [SerializeField] private Vector2 startPos = Vector2.zero;
    [SerializeField] private Vector2 step = new Vector2(100f, 0f);
    [SerializeField] private int maxPerRow = 4;

    private int count = 0;
    private RectTransform Parent => stackParent ? stackParent : (RectTransform)transform;

    public void OnDrop(PointerEventData e)
    {
        var drag = e.pointerDrag ? e.pointerDrag.GetComponent<IngrdientController>() : null;
        if (drag == null) return;

        var sausage = drag.GetComponent<SausageItem>();
        if (sausage == null) return;

        drag.MarkDropped();

        // �׸����� ������ ��� ���� ���� + ���� ����
        if (sausage.CurrentSlot != null)
        {
            sausage.CurrentSlot.Vacate();
            sausage.DetachFromGrill();
        }

        if (sausage.IsExactlyBothDone)
        {
            AddToStack(drag); // ���� �״�� ũ�� �����ؼ� �ױ�
            Debug.Log("���� ����: ��� ��Ȯ�� Done �� Plate�� ����");
        }
        else
        {
            sausage.Consume(); // �̿ϼ�/ź �� ���� ���� ����
            Debug.Log("���� ����: Done �ƴ� �� ����");
        }
    }

    private void AddToStack(IngrdientController drag)
    {
        var rect = drag.GetComponent<RectTransform>();
        rect.SetParent(Parent, worldPositionStays: false);

        Vector2 pos = startPos;
        if (maxPerRow > 0)
        {
            int x = count % maxPerRow;
            int y = count / maxPerRow;
            pos += new Vector2(step.x * x, -step.y * y);
        }
        else
        {
            pos += step * count;
        }

        rect.anchoredPosition = pos;
        count++;
    }
}
