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

    #region �ε� ���� �� �̵�
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


    #region �ε� �� �� �̵�
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
        operation.allowSceneActivation = false; // �ε��� ������ �ڵ� �� ��ȯ XXX

        while (!operation.isDone)
        {
            // �ε� ������� �ݿ��ϴ� ����
            // float progress = Mathf.Clamp01(operation.progress / 0.9f);
            // progressBar.value = progress;

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f); // ��� ��ٷȴٰ�
                operation.allowSceneActivation = true; // ��¥ �� ��ȯ
            }

            yield return null;  // �� ������ ��
        }
    }
    #endregion

}
