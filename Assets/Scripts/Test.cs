using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// 도넛 접시 버튼을 클릭하면
// Clciked UI가 생성되는 테스트
public class Test : MonoBehaviour
{
    [SerializeField] private GameObject image;

    private void Start()
    {
        image.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ClickPlateTest();
        }
    }

    private void ClickPlateTest()
    {
        // 마우스 위치
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);

        // raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePos2, Vector2.zero);

        if(hit.collider != null)
        {
            Debug.Log($"hit collider 정보 : {hit.collider.gameObject.name}");

            if(hit.collider.gameObject.name == "Plate")
            {
                Debug.Log("접시를 눌렀습니다");
                image.SetActive(true);
            }
        }
    }

}
