using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private GameObject targetGameObject;

    private void Awake()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => targetGameObject.SetActive(false));
    }
}
