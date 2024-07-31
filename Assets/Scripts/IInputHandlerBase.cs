using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandlerBase
{
    bool isInputdown { get; } // ���콺 �Է� Down ���� Ȯ��
    bool isInputUp { get; } // ���콺 �Է� Up ���� Ȯ��

    Vector2 inputPosition { get; } // ��ũ�� �ȼ� ��ǥ
}
