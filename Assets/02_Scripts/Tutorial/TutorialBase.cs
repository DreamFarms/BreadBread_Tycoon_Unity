using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    public abstract void Enter();

    // Ʃ�丮�� ���� ���� ���� �� �����Ӹ��� ȣ����
    public abstract void Excute(TutorialController controller);
    public abstract void Exit();

}
