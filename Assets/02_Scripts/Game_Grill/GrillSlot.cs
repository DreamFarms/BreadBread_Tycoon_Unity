using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrillSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform snapPoint; // ���� �ڱ� RectTransform
    [SerializeField] private float flipCooldown = 0.15f;

    public SausageItem Occupant { get; private set; }

    private float lastFlipTime = -999f;
    private RectTransform Rect => (RectTransform)transform;

    public void OnDrop(PointerEventData e)
    {
        var drag = e.pointerDrag ? e.pointerDrag.GetComponent<IngrdientController>() : null;
        if (drag == null || Occupant != null) return; // �̹� �� ������ ����

        var sausage = drag.GetComponent<SausageItem>();
        if (sausage == null) return;

        // ���� & ��� ���� ǥ��
        drag.SnapTo(snapPoint ? snapPoint : Rect);
        drag.MarkDropped();

        // ���� ���� + ���� ����
        Occupant = sausage;
        sausage.AttachToGrill(this);
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (Occupant == null) return;
        if (!Occupant.IsOnGrill) return;
        if (Time.unscaledTime - lastFlipTime < flipCooldown) return; // ����Ŭ�� Ƣ�� �� ����

        Occupant.Flip();
        lastFlipTime = Time.unscaledTime;
    }

    // ����/ȸ�� �� ���� �����
    public void Vacate()
    {
        Occupant = null;
    }
}
