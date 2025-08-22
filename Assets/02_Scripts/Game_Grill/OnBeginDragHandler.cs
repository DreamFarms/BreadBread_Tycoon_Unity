using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnBeginDragHandler : MonoBehaviour, IBeginDragHandler
{
    private Vector3 defaultPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPosition = this.transform.position;

        GetComponent<Image>().raycastTarget = false;
    }

    public Vector3 GetDefaultPosition()
    {
        return defaultPosition;
    }
}
