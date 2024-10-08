using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MilkDrop : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private float dropSpeed;
    [SerializeField] private float initPositionY;
    [SerializeField] private float targetPositionY;

    public void DropMilk()
    {
        StartCoroutine(CoDropMilk());
    }

    private IEnumerator CoDropMilk()
    {
        Vector3 currentTransform = transform.position;

        while(transform.position.y >= targetPositionY)
        {
            // 아래로 이동
            currentTransform.y -= dropSpeed * Time.deltaTime;

            // 위치 업데이트
            currentTransform = currentTransform;
            transform.position = currentTransform;
           yield return null;
        }

        transform.position = new Vector3(0, initPositionY, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MilkBottle"))
        {
            MilkGameManager.Instance.FillMilkBottle();
        }
    }
}
