using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSelector : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    /// <summary>
    /// 셀렉터의 위치를 변경합니다.
    /// </summary>
    public void SetPosition(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
    }

}
