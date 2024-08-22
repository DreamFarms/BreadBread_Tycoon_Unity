using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Dictionary<string, int> userIngredient = new Dictionary<string, int>();
}
