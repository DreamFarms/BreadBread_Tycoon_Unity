using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngrdientController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //Vector3 DefaultPos;

    //void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    //{
    //    DefaultPos = this.transform.position;
    //    GetComponent<Image>().raycastTarget = false;
    //}

    //void IDragHandler.OnDrag(PointerEventData eventData)
    //{
    //    Vector2 currentPos = eventData.position;

    //    this.transform.position = currentPos;
    //}

    //void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    //{
    //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    this.transform.position = DefaultPos;
    //    GetComponent<Image>().raycastTarget = true;
    //}

    [SerializeField] private Canvas canvas;
    private RectTransform rect;
    private CanvasGroup cg;
    private Vector2 defaultPos;
    private bool droppedThisFrame;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        droppedThisFrame = false;
        defaultPos = rect.anchoredPosition;
        if (cg) cg.blocksRaycasts = false; // 드랍존으로 레이캐스트 통과
        transform.SetAsLastSibling();      // 위로 올리기
    }

    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            e.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out var local
        );
        rect.anchoredPosition = local;
    }

    public void OnEndDrag(PointerEventData e)
    {
        // 드랍 안 됐으면 원위치로
        if (!droppedThisFrame) rect.anchoredPosition = defaultPos;
        if (cg) cg.blocksRaycasts = true;
    }

    // DropZone에서 호출
    public void MarkDropped() => droppedThisFrame = true;

    // 스냅(부모를 스냅포인트로 옮기고 로컬(0,0)에 정렬)
    public void SnapTo(RectTransform snapPoint)
    {
        rect.SetParent(snapPoint, worldPositionStays: false);
        rect.anchoredPosition = Vector2.zero;
    }
}
