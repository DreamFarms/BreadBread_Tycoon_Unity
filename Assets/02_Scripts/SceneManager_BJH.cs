using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//public enum SceneName
//{
//    SampleScene,
//    Map,
//    Game_Card,
//    Game_BerryPicker,
//    Game_BakeBread
//}

public class SceneManager_BJH : MonoBehaviour
{
    private static SceneManager_BJH _instance;

    public static SceneManager_BJH Instance
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

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
