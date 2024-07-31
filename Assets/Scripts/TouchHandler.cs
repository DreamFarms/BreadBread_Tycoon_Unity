using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : IInputHandlerBase
{
    public bool isInputdown => Input.GetTouch(0).phase == TouchPhase.Began;

    public bool isInputUp => Input.GetTouch(0).phase == TouchPhase.Ended;

    public Vector2 inputPosition => Input.GetTouch(0).position;
}
