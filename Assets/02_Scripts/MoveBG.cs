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
        // repeat : 주기적인 값 생성. 두 인자를 받아 값을 반복
        // time.time * speed : 시간의 흐름
        // width : 반복 할 범위의 크기ㄴ
        float newPosition = Mathf.Repeat(Time.time * speed, width);

        transform.position = startPosition + Vector2.left * newPosition;

        if(transform.position.x <= -15f)
        {
            transform.position = startPosition;
            newPosition = Mathf.Repeat(Time.time * speed, width);
        }
    }
}
