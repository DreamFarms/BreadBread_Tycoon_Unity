using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrillSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform snapPoint; // 비우면 자기 RectTransform
    [SerializeField] private float flipCooldown = 0.15f;

    public SausageItem Occupant { get; private set; }

    private float lastFlipTime = -999f;
    private RectTransform Rect => (RectTransform)transform;

    public void OnDrop(PointerEventData e)
    {
        var drag = e.pointerDrag ? e.pointerDrag.GetComponent<IngrdientController>() : null;
        if (drag == null || Occupant != null) return; // 이미 차 있으면 무시

        var sausage = drag.GetComponent<SausageItem>();
        if (sausage == null) return;

        // 스냅 & 드랍 성공 표시
        drag.SnapTo(snapPoint ? snapPoint : Rect);
        drag.MarkDropped();

        // 슬롯 점유 + 굽기 시작
        Occupant = sausage;
        sausage.AttachToGrill(this);
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (Occupant == null) return;
        if (!Occupant.IsOnGrill) return;
        if (Time.unscaledTime - lastFlipTime < flipCooldown) return; // 더블클릭 튀는 것 방지

        Occupant.Flip();
        lastFlipTime = Time.unscaledTime;
    }

    // 제출/회수 시 슬롯 비우기용
    public void Vacate()
    {
        Occupant = null;
    }
}
