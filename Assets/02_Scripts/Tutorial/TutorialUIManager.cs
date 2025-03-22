using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{

    public void OnClickNextScriptButton()
    {
        TutorialManager.Instance.NextScript();
    }

}
