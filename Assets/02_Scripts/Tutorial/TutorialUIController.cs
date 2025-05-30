using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialUIController : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public Button nextButton;

    [Header("Guide UI")]
    public GameObject guidePanel;
    public TMP_Text guideText;

    public void Awake()
    {
        guidePanel.SetActive(false);

    }

    public void ShowDialogue(string text, UnityEngine.Events.UnityAction onNext)
    {
        dialogueText.text = text;
        dialoguePanel.SetActive(true);

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(onNext);
        nextButton.gameObject.SetActive(true);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowGuide(string text)
    {
        guideText.text = text;
        guidePanel.SetActive(true);
    }

    public void HideGuide()
    {
        guidePanel.SetActive(false);
    }
}
