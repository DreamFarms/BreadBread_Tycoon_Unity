using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    public abstract void Enter();

    // 튜토리얼 진행 중일 때는 매 프레임마다 호출함
    public abstract void Excute(TutorialController controller);
    public abstract void Exit();

}
