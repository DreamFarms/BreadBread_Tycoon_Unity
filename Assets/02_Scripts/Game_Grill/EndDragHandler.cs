using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndDragHandler : MonoBehaviour, IEndDragHandler
{
    public OnBeginDragHandler beginDragHandler;

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = beginDragHandler.GetDefaultPosition();

        GetComponent<Image>().raycastTarget = true;
    }
}
