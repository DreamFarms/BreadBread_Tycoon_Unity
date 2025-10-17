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

    #region ������ǥ��
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
    //    if (cg) cg.blocksRaycasts = false; // ��������� ����ĳ��Ʈ ���
    //    transform.SetAsLastSibling();      // ���� �ø���
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
    //    // ��� �� ������ ����ġ��
    //    if (!droppedThisFrame) rect.anchoredPosition = defaultPos;
    //    if (cg) cg.blocksRaycasts = true;
    //}

    //// DropZone���� ȣ��
    //public void MarkDropped() => droppedThisFrame = true;

    //// ����(�θ� ��������Ʈ�� �ű�� ����(0,0)�� ����)
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
        canvas = canvas ? canvas.rootCanvas : null; // ��Ʈ ĵ���� ���� ��ǥ ó�� ����
    }

    public void OnBeginDrag(PointerEventData e)
    {
        droppedThisFrame = false;

        // ���� �θ�/��ġ ����
        originalParent = rect.parent as RectTransform;
        originalAnchoredPos = rect.anchoredPosition;

        // ���� �θ� ��ǥ�迡�� ���콺�� ���� ��ǥ
        var cam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            originalParent, e.position, cam, out var mouseLocalOnParent);

        // ���콺�� ������ ���� ������ ����
        pointerOffset = rect.anchoredPosition - mouseLocalOnParent;

        // �巡�� �߿� ��Ʈ ĵ������ �Ű� �� ���� ���̰�(��ǥ�� ���� �� worldPositionStays=false)
        rect.SetParent(canvas.transform, worldPositionStays: false);
        if (cg) cg.blocksRaycasts = false; // ������� ����ĳ��Ʈ�� ���� �� �ְ�
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData e)
    {
        var parentRect = rect.parent as RectTransform;
        var cam = (canvas && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null;

        // "���� �θ�(=��Ʈ ĵ����)" ��ǥ�迡�� ���콺 ��ġ ���
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, e.position, cam, out var mouseLocal);

        // �������� �ݿ��� �ε巴�� ���󰡰�
        rect.anchoredPosition = mouseLocal + pointerOffset;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (cg) cg.blocksRaycasts = true;

        // ����� �����ߴٸ� ���� �θ�/��ġ�� ����
        if (!droppedThisFrame)
        {
            rect.SetParent(originalParent, worldPositionStays: false);
            rect.anchoredPosition = originalAnchoredPos;
        }
        // ���������� ����Ǹ� DropZone �ʿ��� SnapTo�� �θ�/��ġ�� �̹� �������� ��
    }

    // DropZone���� ���� ��� �� ȣ��
    public void MarkDropped() => droppedThisFrame = true;

    // ���� ��� �� ����(��������� ȣ��)
    public void SnapTo(RectTransform snapPoint)
    {
        rect.SetParent(snapPoint, worldPositionStays: false);
        rect.anchoredPosition = Vector2.zero;
    }
}
