using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkDrop : MonoBehaviour
{
    [SerializeField] public float dropSpeed;
    [SerializeField] public float initPositionY;
    [SerializeField] public float targetPositionY;

    public void DropMilk()
    {
        StartCoroutine(CoDropMilk());
    }

    private IEnumerator CoDropMilk()
    {
        Vector3 currentTransform = transform.position;

        while(transform.position.y >= targetPositionY)
        {
            Debug.Log("코루틴 실행");    
            // 아래로 이동
            currentTransform.y -= dropSpeed * Time.deltaTime;

            // 위치 업데이트
            currentTransform = currentTransform;
            transform.position = currentTransform;
           yield return null;
        }

        transform.position = new Vector3(0, initPositionY, 0);
    }
}
