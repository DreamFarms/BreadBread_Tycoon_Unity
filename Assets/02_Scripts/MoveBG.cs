using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBG : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 startPosition;
    private float width;

    private void Start()
    {
        startPosition = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
    }

    private void Update()
    {
        // repeat : �ֱ����� �� ����. �� ���ڸ� �޾� ���� �ݺ�
        // time.time * speed : �ð��� �帧
        // width : �ݺ� �� ������ ũ�⤤
        float newPosition = Mathf.Repeat(Time.time * speed, width);

        transform.position = startPosition + Vector2.left * newPosition;

        if(transform.position.x <= -15f)
        {
            transform.position = startPosition;
            newPosition = Mathf.Repeat(Time.time * speed, width);
        }
    }
}
