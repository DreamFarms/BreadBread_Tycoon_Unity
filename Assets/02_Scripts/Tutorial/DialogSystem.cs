using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private Dialog[] dialogs;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scriptText;
    [SerializeField] private float typingSpeed;

    int currentIndex = -1;
    string currentScript;
    bool isTypingEffect = false;

    public void StartDialog()
    {
        SetNextDialog();
    }

    public bool UpdateDialog()
    {
        if(isTypingEffect)
        {
            StopCoroutine("TypingText");
            isTypingEffect = false;
            scriptText.text = currentScript;

            return false;
        }

        if(dialogs.Length > currentIndex + 1)
        {
            SetNextDialog();
        }
        else
        {
            return true;
        }

        return false;
    }

    private void SetNextDialog()
    {
        currentIndex++;

        string speakerName = dialogs[currentIndex].speakerName;
        string script = dialogs[currentIndex].script;
        currentScript = script;

        nameText.text = speakerName;

        StartCoroutine(TypingText(script));
    }

    private IEnumerator TypingText(string script)
    {
        int index = 0;

        isTypingEffect = true;

        while(index < script.Length)
        {
            scriptText.text = script.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTypingEffect = false;
    }
}


