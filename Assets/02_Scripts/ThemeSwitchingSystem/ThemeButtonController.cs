using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI 버튼 이벤트에서 연결할 함수
public class ThemeButtonController : MonoBehaviour
{
    [SerializeField] private StoreThemeManager storeThemeManager;

    [Header("버튼-테마 매핑 리스트")]
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

