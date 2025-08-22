using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform snapPoint;

    public void OnDrop(PointerEventData e)
    {
        Debug.Log($"[DropZone] OnDrop called on {name}, pointerDrag={(e.pointerDrag ? e.pointerDrag.name : "null")}");

        var drag = e.pointerDrag ? e.pointerDrag.GetComponent<IngrdientController>() : null;
        if (drag == null) return;

        // 1) 드랍됨 표시해서 OnEndDrag가 원위치로 되돌리지 않게
        drag.MarkDropped();

        // 2) 스냅 수행
        drag.SnapTo(snapPoint ? snapPoint : (transform as RectTransform));
    }
}