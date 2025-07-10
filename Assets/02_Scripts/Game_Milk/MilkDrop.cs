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
            // �Ʒ��� �̵�
            currentTransform.y -= dropSpeed * Time.deltaTime;

            // ��ġ ������Ʈ
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
