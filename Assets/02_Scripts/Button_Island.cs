using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Island : Button
{
    // Start is called before the first frame update
    void Start()
    {
        this.onClick.AddListener(CallSceneManagerAndPlzChangeScene);
    }

    public void CallSceneManagerAndPlzChangeScene()
    {
        AudioManager.Instance.StopBGM();
        SceneManager_BJH.Instance.ChangeScene(gameObject.name);
    }
}
