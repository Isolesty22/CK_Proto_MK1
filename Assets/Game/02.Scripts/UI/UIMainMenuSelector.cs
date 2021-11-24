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
        [Tooltip("누르지 않고 선택만 된 상태의 이미지")]
        public Image select_off;

        [Tooltip("눌렀을 때의 이미지")]
        public Image select_on;
    }
    [SerializeField]
    private RectTransform rect;

    [SerializeField]
    private Images image;

    /// <summary>
    /// 셀렉터의 위치를 변경합니다.
    /// </summary>
    public void SetPosition(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
    }

    /// <summary>
    /// true = On, false = off로 변경합니다. \n
    /// 버튼이 눌렸을 때 On, 그냥 선택만 되었을 때 Off입니다.
    /// </summary>
    /// <param name="_b"></param>
    public void SetImageType(bool _b)
    {

    }

}
