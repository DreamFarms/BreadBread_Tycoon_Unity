using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // ��ġ or ���콺�� �Էµ� ��ũ�� ��ǥ�� ������ 0,0(����)�� ���������ϴ� Local ��ǥ�� ��ȯ�ϱ� ���ؼ�
    // Gameobject�� ���۷����� �ʿ���
    Transform _container; 

#if UNITY_EDITOR
    IInputHandlerBase _inputHandler = new MouseHandler();
#elif UNITY_ANDROID
    IInputHandlerBase _inputHandler = new TouchHandler();
#endif

    // ������
    // �����̳�(board gameobject)�� ���� ����
    public InputManager(Transform container)
    {
        this._container = container;
    }

    public bool isTouchDown => _inputHandler.isInputdown;
    public bool isTouchUp => _inputHandler.isInputUp;
    public Vector2 touchPosition => _inputHandler.inputPosition; // screen ��ǥ ��û

    // board�� ������ �������� ��ȯ�� local ��ǥ�� ��ȯ
    public Vector2 touch2BoardPosition => TouchToPosition(_inputHandler.inputPosition);

    // ��ũ�� ��ǥ�� ���� ���� ���� ���� ��ǥ�� ��ȯ
    // ��ũ�� ��ǥ �� �μ� ��ǥ�� �ް�
    // ���� ���� ���� ��ǥ�� ����
    Vector2 TouchToPosition(Vector3 vtInput)
    {
        //1. ��ũ�� ��ǥ -> ���� ��ǥ
        Vector3 vtMousePosW = Camera.main.ScreenToWorldPoint(vtInput);

        //2. �����̳� local ��ǥ��� ��ȯ(�����̳� ��ġ �̵��ÿ��� �����̳� ������ ���� ��ǥ���̹Ƿ� ȭ�� ������ �����ϴ�)
        Vector3 vtContainerLocal = _container.transform.InverseTransformPoint(vtMousePosW);

        return vtContainerLocal;
    }
}
