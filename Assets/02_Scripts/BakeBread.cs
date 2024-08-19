using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeBread : MonoBehaviour
{
    [SerializeField] private GameObject[] doughList = new GameObject[4];
    [SerializeField] private Transform[] doughTransformList = new Transform[4];

    private void Start()
    {
        foreach(GameObject go in doughList)
        {
            if(go == null)
            {
                
            }
        }
    }


}
