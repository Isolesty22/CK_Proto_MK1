using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuSelector : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    /// <summary>
    /// �������� ��ġ�� �����մϴ�.
    /// </summary>
    public void SetPosition(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
    }

}
