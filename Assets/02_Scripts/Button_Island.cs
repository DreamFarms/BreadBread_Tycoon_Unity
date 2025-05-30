using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Island : Button
{
    [SerializeField] private string tutorialEventName;
    [SerializeField] private bool isTutorial = false;

    void Start()
    {
        this.onClick.AddListener(CallSceneManagerAndPlzChangeScene);
    }

    public void CallSceneManagerAndPlzChangeScene()
    {
        if(this.isTutorial)
        {
            // TutorialManager.Instance.iswai
        }
        SceneManager_BJH.Instance.ChangeScene(gameObject.name);
    }
}
