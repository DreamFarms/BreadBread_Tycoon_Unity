using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // 터치 or 마우스로 입력된 스크린 좌표를 보드의 0,0(원점)을 기준으로하는 Local 좌표로 변환하기 위해서
    // Gameobject의 레퍼런스가 필요함
    Transform _container; 

#if UNITY_EDITOR
    IInputHandlerBase _inputHandler = new MouseHandler();
#elif UNITY_ANDROID
    IInputHandlerBase _inputHandler = new TouchHandler();
#endif

    // 생성자
    // 컨테이너(board gameobject)를 전달 받음
    public InputManager(Transform container)
    {
        this._container = container;
    }

    public bool isTouchDown => _inputHandler.isInputdown;
    public bool isTouchUp => _inputHandler.isInputUp;
    public Vector2 touchPosition => _inputHandler.inputPosition; // screen 좌표 요청

    // board의 원점을 기준으로 변환된 local 좌표를 반환
    public Vector2 touch2BoardPosition => TouchToPosition(_inputHandler.inputPosition);

    // 스크린 좌표를 씬의 보드 기준 로컬 좌표로 변환
    // 스크린 좌표 즉 인섹 좌표를 받고
    // 보드 기준 로컬 좌표를 리턴
    Vector2 TouchToPosition(Vector3 vtInput)
    {
        //1. 스크린 좌표 -> 월드 좌표
        Vector3 vtMousePosW = Camera.main.ScreenToWorldPoint(vtInput);

        //2. 컨테이너 local 좌표계로 변환(컨테이너 위치 이동시에도 컨테이너 기준의 로컬 좌표계이므로 화면 구성이 유연하다)
        Vector3 vtContainerLocal = _container.transform.InverseTransformPoint(vtMousePosW);

        return vtContainerLocal;
    }
}
