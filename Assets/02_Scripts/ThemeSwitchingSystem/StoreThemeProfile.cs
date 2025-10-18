using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoreThemeType
{
    Default,
    Honey,
    Rainbow,
    Cookie,
    Made,
    Sky
}

[CreateAssetMenu(menuName = "Store/StoreThemeProfile")]
public class StoreThemeProfile : ScriptableObject
{
    public StoreThemeType themeType;

    [Header("Prefab for this theme")]
    public GameObject themePrefab;

    // [Header("Visual settings")]
    // public Color ambientColor;
    // public AudioClip ambientMusic;
}
