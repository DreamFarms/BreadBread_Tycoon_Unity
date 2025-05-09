using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string speakerName;

    [TextArea(3, 5)]
    public string script;
}