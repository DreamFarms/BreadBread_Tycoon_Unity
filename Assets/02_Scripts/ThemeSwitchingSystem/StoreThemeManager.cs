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

        // ���� �ν��Ͻ� ����
        if (_currentInstance != null)
            Destroy(_currentInstance);

        // �� �׸� ������ �ν��Ͻ�
        _currentInstance = Instantiate(profile.themePrefab, themeRoot);

        // ȯ�� ���� / ���� ����
        // RenderSettings.ambientLight = profile.ambientColor;
        // BGMManager.Instance.Play(profile.ambientMusic);

        _currentTheme = themeType;
    }
}
