using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ���� ���� ��ư�� Ŭ���ϸ�
// Clciked UI�� �����Ǵ� �׽�Ʈ
// Clicked UI���� Plate�� ������ �ش��ϴ� �޴��� ���� ���� ������
public class Plate : MonoBehaviour
{
    // ���õ� ���� ���� ����
    [SerializeField] private GameObject _selectedPlate;
    public bool isPlate {get; private set; }
    public string menuName { get; private set; }

    private void Start()
    {
        isPlate = false;
        menuName = string.Empty;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ClickPlateTest();
        }
    }

    // ���ø� Ŭ���ϸ�, �޴� ���� UI�� �ߴ� �޼���
    private void ClickPlateTest()
    {
        // ���콺 ��ġ
        Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2 = new Vector2(mousePos3.x, mousePos3.y);

        // raycast
        RaycastHit2D hit = Physics2D.Raycast(mousePos2, Vector2.zero);

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.CompareTag("Plate"))
            {
                _selectedPlate = hit.collider.gameObject;
                UIManager um = UIManager.Instance;
                um.OnActiveThisUI(um.selectPlateMenuImg);
                um.OnActiveThisUI(um.breadInfoImgGroup);
            }
        }
    }

    // �޴��� �����ϸ� ���õ� �޴��� �����ǰ�
    // UI�� "���õ�" ǥ�ð� ��Ÿ���� �޼���
    // ��ư �����θ� GameObject�� �Ű������� ��´�.
    public void OnclickMenuBtn(GameObject go)
    {
        go.GetComponent<Image>().color = new Color(0.45f, 0.45f, 0.45f);
    }

    // ���õ� ���ÿ� �޴��� �����ϴ� �޼���
    public void PlateSelectedMenu(string menuName, GameObject plate)
    {
        print("inPlateHere");
        Sprite sprite = Resources.Load<Sprite>("Breads/" + menuName);
        // �̽� : _selectedPlate���� null�� �߻�. �и� Plate ���ӿ�����Ʈ�� �Ҵ� �� ����
        // �ذ��� �ȵǼ� find�� ã�� ����. ������ ���� ������ �����Ƿ� �� �ذ� �ؾߵ�
        GameObject childGo = plate.transform.GetChild(0).gameObject;
        SpriteRenderer spriteRenderer = childGo.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            GameObject go = new GameObject("Food");
            Instantiate(go, _selectedPlate.transform);
            _selectedPlate.AddComponent<SpriteRenderer>();
        }
        // ���� �ö󰡴� �κ� ���̾ 3���� ����
        spriteRenderer.sortingOrder = 3;
        spriteRenderer.sprite = sprite;
        isPlate = true;
        this.menuName = menuName;
        print("inPlateHere");
    }

    public void ReturnSelectMenu(GameObject plate)
    {
        GameObject childGo = plate.transform.GetChild(0).gameObject;
        SpriteRenderer spriteRenderer = childGo.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
        isPlate = false;
    }

    

}
