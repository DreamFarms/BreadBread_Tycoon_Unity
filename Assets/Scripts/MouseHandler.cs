using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : IInputHandlerBase
{
    public bool isInputdown => Input.GetButtonDown("Down");

    public bool isInputUp => Input.GetButtonUp("Up");

    public Vector2 inputPosition => Input.mousePosition;
}
