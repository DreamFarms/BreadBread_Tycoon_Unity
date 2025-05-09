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
        // ��� ����
        bool isCompleted = dialogSystem.UpdateDialog();


        // ��� ��
        if (isCompleted)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }
}
