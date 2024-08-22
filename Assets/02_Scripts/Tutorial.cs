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
        // 튜토리얼 버튼을 누르면 튜토리얼이 활성화
        tutorialButton.onClick.AddListener(() => tutorial.SetActive(true));
        preButton.onClick.AddListener(() => OnclickNextTutorial(-1));
        nextButton.onClick.AddListener(() => OnclickNextTutorial(1));
        closeButton.onClick.AddListener(() => tutorial.SetActive(false));


        // 기본적으로 튜토리얼은 비활성화
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
        // 이전 페이지 끄고, 선택한 페이지를 켠다.
        tutorials[page].SetActive(false);

        // 페이지 업데이트
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
