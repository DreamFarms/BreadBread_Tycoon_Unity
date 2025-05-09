using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogSystem))]
public class TutorialDialog : TutorialBase
{
    private DialogSystem dialogSystem;
    public override void Enter()
    {
        dialogSystem = GetComponent<DialogSystem>();
        dialogSystem.StartDialog();
    }

    public override void Excute(TutorialController controller)
    {
        // 대사 진행
        bool isCompleted = dialogSystem.UpdateDialog();


        // 대사 끝
        if (isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
