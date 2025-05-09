using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    // 튜토리얼 목록
    [SerializeField] private List<TutorialBase> tutorials;

    [SerializeField] private string nextSceneName;

    private TutorialBase currentTutorial;
    private int currentTutorialIndex = -1;

    private void Start()
    {
        SetNextTutorial();
    }

    private void Update()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Excute(this);
        }
    }

    public void SetNextTutorial()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        if(currentTutorialIndex >= tutorials.Count)
        {
            CompletedAllTutorial();
            return;
        }

        currentTutorialIndex++;
        currentTutorial = tutorials[currentTutorialIndex];

        currentTutorial.Enter();
    }

    private void CompletedAllTutorial()
    {
        currentTutorial = null;

        Debug.Log("모든 튜토리얼을 마쳤습니다. 씬을 이동합니다.");

        if(!nextSceneName.Equals(""))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}