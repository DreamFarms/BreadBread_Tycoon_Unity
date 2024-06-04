using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// ���� ���� ��ư�� Ŭ���ϸ�
// Clciked UI�� �����Ǵ� �׽�Ʈ
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
        // ���콺 ��ġ
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);

        // raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePos2, Vector2.zero);

        if(hit.collider != null)
        {
            Debug.Log($"hit collider ���� : {hit.collider.gameObject.name}");

            if(hit.collider.gameObject.name == "Plate")
            {
                Debug.Log("���ø� �������ϴ�");
                image.SetActive(true);
            }
        }
    }

}
