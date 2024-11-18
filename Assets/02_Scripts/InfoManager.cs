using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
    private static InfoManager _instance;

    public Dictionary<string, string> enKoMappingDic = new Dictionary<string, string>();

    public static InfoManager Instance
    { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("dsdsd");
            foreach (var pair in enKoMappingDic)
            {
                Debug.Log("in_foreach");
                Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
            }
            print(enKoMappingDic.Count);
        }
    }

    //public string nickName;
    //public int coin;
    //public int cash;
    //public float timer;
    //public Dictionary<string, int> userIngredient = new Dictionary<string, int>();

    //private void Start()
    //{
    //    timer = 300;
    //}

    //private void Update()
    //{
    //    if (!SceneManager.GetActiveScene().name.Equals("Main"))
    //    {
    //        timer -= Time.deltaTime;
    //        if (timer <= 0)
    //        {
    //            timer = 0;
    //        }
    //    }
    //}
}
