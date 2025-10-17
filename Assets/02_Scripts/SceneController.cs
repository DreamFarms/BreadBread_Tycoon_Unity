using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    GoogleLogin,
    Main_Tmp,
    Map,
    Game_Card,
    Game_BerryPicker,
    Game_BakeBread,
    Game_Milk,
    Game_Cut,
    Collection_Book
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;
    
    private static SceneController _instance;

    public static SceneController Instance
    {
        get { return _instance; }
    }



    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region 로딩 없이 씬 이동
    public void ChangeScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(++index);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeScene(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
    #endregion


    #region 로딩 후 씬 이동
    public void LoadSceneWithLoading()
    {
        int targetSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        string targetSceneName = SceneManager.GetSceneByBuildIndex(targetSceneIndex).name;
        StartCoroutine(LoadSceneAsync(targetSceneName));
    }

    public void LoadSceneWithLoading(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadSceneWithLoading(SceneName sceneName)
    {
        string targetSceneName = sceneName.ToString();
        StartCoroutine(LoadSceneAsync(targetSceneName));
    }



    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingUI.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // 로딩이 끝나도 자동 씬 전환 XXX

        while (!operation.isDone)
        {
            // 로딩 진행률을 반영하는 로직
            // float progress = Mathf.Clamp01(operation.progress / 0.9f);
            // progressBar.value = progress;

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); // 잠깐 기다렸다가
                operation.allowSceneActivation = true; // 진짜 씬 전환
            }

            yield return null;  // 한 프레임 쉼
        }
    }
    #endregion

}
