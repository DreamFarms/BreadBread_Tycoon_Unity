using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemBundleBaseData Data", menuName = "Scriptable Object/ItemBundleBaseData Data", order = int.MaxValue)]

public class MenuBundleBaseData : ScriptableObject
{
    [SerializeField] private Sprite[] _itemIcon; // 아이콘에 사용 될 sprite 배열
    public Sprite[] itemIcon { get { return _itemIcon; } }
    [SerializeField]
    [TextArea(12, 20)] // TextArea는 데이터 가공에 줄바꿈을 인식
    private string _data; // 콤마 분활 형식의 통텍스트가 입력되는 변수
    public string data { get { return _data; } }

    // 스크립터블 오브젝트를 만들어서 data에 엑셀 추출 값을 넣고,
    // 해당되는 아이콘 ItemIcon배열에 순서대로 할당
    // 아이콘 sprite를 담을 때는 properties를 활용
    
}
