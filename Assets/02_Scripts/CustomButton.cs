using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    // 해당 버튼을 클릭하면 매개변수 오브젝트 상태가 반전됨
    // on -> off
    // off -> on
    public void Onclick_ChangeUIState(GameObject go)
    {
        bool state = go.activeSelf;
        go.SetActive(!state);
    }
}
