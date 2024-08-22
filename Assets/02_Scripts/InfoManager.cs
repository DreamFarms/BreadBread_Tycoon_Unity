using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{
    private static InfoManager _instance;

    public static InfoManager Instance
    { get { return _instance; } }

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

    public string nickName;
    public int coin;
    public int cash;
    public float timer;
    public Dictionary<string, int> userIngredient = new Dictionary<string, int>();

    private void Start()
    {
        timer = 300;
    }

    private void Update()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Main"))
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = 0;
            }
        }
    }
}
