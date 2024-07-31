using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandlerBase
{
    bool isInputdown { get; } // 마우스 입력 Down 상태 확인
    bool isInputUp { get; } // 마우스 입력 Up 상태 확인

    Vector2 inputPosition { get; } // 스크린 픽셀 좌표
}
