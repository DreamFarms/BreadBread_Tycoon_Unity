using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemBundleBaseData Data", menuName = "Scriptable Object/ItemBundleBaseData Data", order = int.MaxValue)]

public class MenuBundleBaseData : ScriptableObject
{
    [SerializeField] private Sprite[] _itemIcon; // �����ܿ� ��� �� sprite �迭
    public Sprite[] itemIcon { get { return _itemIcon; } }
    [SerializeField]
    [TextArea(12, 20)] // TextArea�� ������ ������ �ٹٲ��� �ν�
    private string _data; // �޸� ��Ȱ ������ ���ؽ�Ʈ�� �ԷµǴ� ����
    public string data { get { return _data; } }

    // ��ũ���ͺ� ������Ʈ�� ���� data�� ���� ���� ���� �ְ�,
    // �ش�Ǵ� ������ ItemIcon�迭�� ������� �Ҵ�
    // ������ sprite�� ���� ���� properties�� Ȱ��
    
}
