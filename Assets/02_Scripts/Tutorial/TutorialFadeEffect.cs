using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFadeEffect : TutorialBase
{
    // [SerializeField] private FadeEffect fadeEffect;
    [SerializeField] private bool isFadeIn = false;
    private bool isCompleted = false;

    public override void Enter()
    {
        if(isFadeIn)
        {
            // fadeEffect.FadeIn(OnAfterFadeEffect);
        }
        else
        {
            // fadeEffect.FadeOut(OnAfterFadeEffect);
        }
    }

    private void OnAfterFadeEffect()
    {
        isCompleted = true;
    }

    public override void Excute(TutorialController controller)
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
