using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    Main,
    Card,
    BerryPicker,
    BakeBread
}

public class AudioManager : MonoBehaviour
{
    #region Instance
    private static AudioManager _instance;

    public static AudioManager Instance
    { get { return _instance; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }    
    }
    #endregion

    [SerializeField] private List<AudioClip> BGM = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;

    public void PlayBGM(BGM bgm)
    {
        int index = (int)bgm;
        audioSource.clip = BGM[index];
        audioSource.Play();
    }
}
