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
        if (cg) cg.blocksRaycasts = false; // ��������� ����ĳ��Ʈ ���
        transform.SetAsLastSibling();      // ���� �ø���
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
        // ��� �� ������ ����ġ��
        if (!droppedThisFrame) rect.anchoredPosition = defaultPos;
        if (cg) cg.blocksRaycasts = true;
    }

    // DropZone���� ȣ��
    public void MarkDropped() => droppedThisFrame = true;

    // ����(�θ� ��������Ʈ�� �ű�� ����(0,0)�� ����)
    public void SnapTo(RectTransform snapPoint)
    {
        rect.SetParent(snapPoint, worldPositionStays: false);
        rect.anchoredPosition = Vector2.zero;
    }
}
