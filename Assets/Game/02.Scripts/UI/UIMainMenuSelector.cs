using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIMainMenuSelector : MonoBehaviour
{
    [Serializable]
    public class Images
    {
        [Tooltip("������ �ʰ� ���ø� �� ������ �̹���")]
        public Image select_off;

        [Tooltip("������ ���� �̹���")]
        public Image select_on;
    }
    [SerializeField]
    private RectTransform rect;

    [SerializeField]
    private Images image;

    /// <summary>
    /// �������� ��ġ�� �����մϴ�.
    /// </summary>
    public void SetPosition(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
    }

    /// <summary>
    /// true = On, false = off�� �����մϴ�. \n
    /// ��ư�� ������ �� On, �׳� ���ø� �Ǿ��� �� Off�Դϴ�.
    /// </summary>
    /// <param name="_b"></param>
    public void SetImageType(bool _b)
    {

    }

}
