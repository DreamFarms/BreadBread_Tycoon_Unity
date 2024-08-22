using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapCanvas mapCanvas;
    private void Start()
    {
        // �ӽ�
        mapCanvas.nicknameText.text = InfoManager.Instance.nickName;
        mapCanvas.coinText.text = "1,000 ��";
        mapCanvas.cashText.text = "0 ��";

        AudioManager.Instance.PlayBGM(BGM.Main);
    }
}
