using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private OnBeginDragHandler beginDragHandler;
    private DragHandler dragHandler;
    private EndDragHandler endDragHandler;

    void Awake()
    {
        // 핸들러 초기화
        beginDragHandler = gameObject.AddComponent<OnBeginDragHandler>();
        dragHandler = gameObject.AddComponent<DragHandler>();
        endDragHandler = gameObject.AddComponent<EndDragHandler>();

        endDragHandler.beginDragHandler = beginDragHandler;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragHandler.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragHandler.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endDragHandler.OnEndDrag(eventData);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
