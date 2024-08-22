using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button tutorialButton;

    public GameObject tutorial;
    public GameObject[] tutorials;
    public Button nextButton;
    public Button preButton;
    public Button closeButton;
    int page;


    private void Start()
    {
        // Ʃ�丮�� ��ư�� ������ Ʃ�丮���� Ȱ��ȭ
        tutorialButton.onClick.AddListener(() => tutorial.SetActive(true));
        preButton.onClick.AddListener(() => OnclickNextTutorial(-1));
        nextButton.onClick.AddListener(() => OnclickNextTutorial(1));
        closeButton.onClick.AddListener(() => tutorial.SetActive(false));


        // �⺻������ Ʃ�丮���� ��Ȱ��ȭ
        tutorial.gameObject.SetActive(false);

        foreach(var tutorial in tutorials)
        {
            tutorial.SetActive(false);
        }

        tutorials[0].SetActive(true);
        page = 0;

    }

    private void OnclickNextTutorial(int value)
    {
        // ���� ������ ����, ������ �������� �Ҵ�.
        tutorials[page].SetActive(false);

        // ������ ������Ʈ
        page += value;

        if(page >= tutorials.Length)
        {
            page = tutorials.Length-1;
            return;
        }

        if(page < 0)
        {
            page = 0;
            return;
        }

        tutorials[page].SetActive(true);

    }

    
}
