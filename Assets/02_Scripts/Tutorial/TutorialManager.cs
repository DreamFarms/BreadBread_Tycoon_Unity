using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager _instance;

    public static TutorialManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] private string[] tutorialScripts = new string[10];
    private int tutorialScriptIndex = 0;
    [SerializeField] private TMP_Text tutorialScriptText;

    private void Awake()
    {
        if(_instance == null)
        {
            SetInstance();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetInstance()
    {
        _instance = this;
    }

    public void NextScript()
    {
        tutorialScriptIndex++;
        string script = tutorialScripts[tutorialScriptIndex];

        tutorialScriptText.text = script;
    }
}
