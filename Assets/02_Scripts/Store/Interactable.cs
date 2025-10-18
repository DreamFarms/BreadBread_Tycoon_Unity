using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject breadPannel;
    [SerializeField] private GameObject counterBox;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("up");
    }
}
