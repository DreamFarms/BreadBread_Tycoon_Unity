using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreThemeManager : MonoBehaviour
{
    [SerializeField] private List<StoreThemeProfile> themeProfiles;
    [SerializeField] private Transform themeRoot;

    private GameObject _currentInstance;
    private StoreThemeType _currentTheme;

    public void ApplyTheme(StoreThemeType themeType)
    {
        if (_currentTheme == themeType) return;

        var profile = themeProfiles.Find(p => p.themeType == themeType);
        if (profile == null)
        {
            Debug.LogWarning($"[Store] No theme profile found for {themeType}");
            return;
        }

        // 이전 인스턴스 제거
        if (_currentInstance != null)
            Destroy(_currentInstance);

        // 새 테마 프리팹 인스턴스
        _currentInstance = Instantiate(profile.themePrefab, themeRoot);

        // 환경 색상 / 사운드 적용
        // RenderSettings.ambientLight = profile.ambientColor;
        // BGMManager.Instance.Play(profile.ambientMusic);

        _currentTheme = themeType;
    }
}
