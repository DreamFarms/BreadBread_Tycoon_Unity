using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngrdientController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rect;
    private CanvasGroup cg;

    private RectTransform originalParent;
    private Vector2 originalAnchoredPos;

    //private Vector2 defaultPos;

    private bool droppedThisFrame;
    private Vector2 pointerOffset;

    #region 절대좌표계
    //private void Awake()
    //{
    //    rect = GetComponent<RectTransform>();
    //    cg = GetComponent<CanvasGroup>();
    //    if (canvas == null) canvas = GetComponentInParent<Canvas>();
    //}

    //public void OnBeginDrag(PointerEventData e)
    //{
    //    droppedThisFrame = false;
    //    defaultPos = rect.anchoredPosition;
    //    if (cg) cg.blocksRaycasts = false; // 드랍존으로 레이캐스트 통과
    //    transform.SetAsLastSibling();      // 위로 올리기
    //}

    //public void OnDrag(PointerEventData e)
    //{
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        (RectTransform)canvas.transform,
    //        e.position,
    //        canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
    //        out var local
    //    );
    //    rect.anchoredPosition = local;
    //}

    //public void OnEndDrag(PointerEventData e)
    //{
    //    // 드랍 안 됐으면 원위치로
    //    if (!droppedThisFrame) rect.anchoredPosition = defaultPos;
    //    if (cg) cg.blocksRaycasts = true;
    //}

    //// DropZone에서 호출
    //public void MarkDropped() => droppedThisFrame = true;

    //// 스냅(부모를 스냅포인트로 옮기고 로컬(0,0)에 정렬)
    //public void SnapTo(RectTransform snapPoint)
    //{
    //    rect.SetParent(snapPoint, worldPositionStays: false);
    //    rect.anchoredPosition = Vector2.zero;
    //}
    #endregion

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        canvas = canvas ? canvas.rootCanvas : null; // 루트 캔버스 쓰면 좌표 처리 편함
    }

    public void OnBeginDrag(PointerEventData e)
    {
        droppedThisFrame = false;

        // 원래 부모/위치 저장
        originalParent = rect.parent as RectTransform;
        originalAnchoredPos = rect.anchoredPosition;

        // 현재 부모 좌표계에서 마우스의 로컬 좌표
        var cam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            originalParent, e.position, cam, out var mouseLocalOnParent);

        // 마우스와 아이템 사이 오프셋 저장
        pointerOffset = rect.anchoredPosition - mouseLocalOnParent;

        // 드래그 중엔 루트 캔버스로 옮겨 맨 위에 보이게(좌표계 변경 시 worldPositionStays=false)
        rect.SetParent(canvas.transform, worldPositionStays: false);
        if (cg) cg.blocksRaycasts = false; // 드랍존이 레이캐스트를 받을 수 있게
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData e)
    {
        var parentRect = rect.parent as RectTransform;
        var cam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

        // "현재 부모(=루트 캔버스)" 좌표계에서 마우스 위치 계산
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, e.position, cam, out var mouseLocal);

        // 오프셋을 반영해 부드럽게 따라가게
        rect.anchoredPosition = mouseLocal + pointerOffset;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (cg) cg.blocksRaycasts = true;

        // 드롭이 실패했다면 원래 부모/위치로 복귀
        if (!droppedThisFrame)
        {
            rect.SetParent(originalParent, worldPositionStays: false);
            rect.anchoredPosition = originalAnchoredPos;
        }
        // 성공적으로 드랍되면 DropZone 쪽에서 SnapTo로 부모/위치를 이미 정리해줄 것
    }

    // DropZone에서 성공 드랍 시 호출
    public void MarkDropped() => droppedThisFrame = true;

    // 성공 드랍 시 스냅(드랍존에서 호출)
    public void SnapTo(RectTransform snapPoint)
    {
        rect.SetParent(snapPoint, worldPositionStays: false);
        rect.anchoredPosition = Vector2.zero;
    }
}
