using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public class KeyChangeButton : MonoBehaviour
{
    [Header("해당하는 키 이름")]
    public string keyType;

    [ReadOnly]
    [Header("현재 키 코드")]
    public KeyCode keyCode;

    public FieldInfo field;

    [HideInInspector]
    public Button button;

    [HideInInspector]
    public Image image;

    [HideInInspector]
    public Text text;

    [HideInInspector]
    public bool isFailed = false;

    public void Init()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        isFailed = false;
    }
}
