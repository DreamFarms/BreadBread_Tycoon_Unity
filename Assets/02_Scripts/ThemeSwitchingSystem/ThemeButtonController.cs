using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI ��ư �̺�Ʈ���� ������ �Լ�
public class ThemeButtonController : MonoBehaviour
{
    [SerializeField] private StoreThemeManager storeThemeManager;

    [Header("��ư-�׸� ���� ����Ʈ")]
    [SerializeField] private List<ThemeButtonBinding> buttonBindings;

    private void Start()
    {
        foreach (var binding in buttonBindings)
        {
            var type = binding.themeType;
            binding.button.onClick.AddListener(() =>
                storeThemeManager.ApplyTheme(type));
        }
    }
}

