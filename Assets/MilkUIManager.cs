using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkUIManager : MonoBehaviour
{
    private static MilkUIManager _instance;

    public static MilkUIManager Instance
    {
        get { return _instance; }
    }

    [SerializeField] private GameObject[] oxSprites; // ox ¿ÃπÃ¡ˆ assign // 0 : false, 1 : true

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private void OnEnable()
    {
        foreach(GameObject go in oxSprites)
        {
            go.SetActive(false);
        }
    }
 

    public void ActiveO()
    {
        oxSprites[1].SetActive(true);
        StartCoroutine(DeActiveOXUI(oxSprites[1]));

    }

    public void ActiveX()
    {
        oxSprites[0].SetActive(true);
        StartCoroutine(DeActiveOXUI(oxSprites[0]));
    }

    private IEnumerator DeActiveOXUI(GameObject go)
    {
        yield return new WaitForSeconds(0.5f);
        go.SetActive(false);
    }

    

}
