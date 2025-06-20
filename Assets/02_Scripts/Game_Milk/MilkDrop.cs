using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MilkDrop : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private float dropSpeed;
    private float initPositionY;
    [SerializeField] private float targetPositionY;

    private void Awake()
    {
        initPositionY = transform.position.y;
    }

    public void DropMilk()
    {
        StartCoroutine(CoDropMilk());
    }

    private IEnumerator CoDropMilk()
    {
        Vector2 currentTransform = transform.position;

        while(transform.position.y >= targetPositionY)
        {
            // 아래로 이동
            currentTransform.y -= dropSpeed * Time.deltaTime;

            // 위치 업데이트
            transform.position = currentTransform;
           yield return null;
        }

        Vector2 initVector = new Vector3(transform.position.x, initPositionY);
        transform.position = initVector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MilkBottle"))
        {
            MilkGameManager.Instance.FillMilkBottle();
        }
    }
}
