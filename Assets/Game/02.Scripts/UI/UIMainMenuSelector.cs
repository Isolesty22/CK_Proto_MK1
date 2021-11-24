using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIMainMenuSelector : MonoBehaviour
{

    [Tooltip("셀렉터에 등록할 버튼들")]
    public Button[] buttons;

    [Tooltip("버튼들의 렉트 트랜스폼")]
    private RectTransform[] buttonRects;

    public int selectIndexOnStart;

    [Serializable]
    public class Images
    {
        [Tooltip("눌렀을 때의 이미지")]
        public Sprite select_on;

        [Tooltip("누르지 않고 선택만 된 상태의 이미지")]
        public Sprite select_off;
    }

    [SerializeField]
    [Tooltip("셀렉터의 이미지입니다.")]
    private Image selectorImage;

    [SerializeField]
    [Tooltip("셀렉터의 렉트 트랜스폼입니다.")]
    private RectTransform rect;

    [SerializeField]
    private Images images;


    [Serializable]
    public class Currents
    {
        public int index;
        public Button button;
        public RectTransform rect;
    }


    public Currents current;

    /// <summary>
    /// 등록된 버튼의 총 개수입니다.
    /// </summary>
    public int buttonsCount { get; private set; }

    private void Awake()
    {
        buttonsCount = buttons.Length;
        buttonRects = new RectTransform[buttonsCount];
        for (int i = 0; i < buttonsCount; i++)
        {

        }

    }


    /// <summary>
    /// 셀렉터의 위치를 변경합니다.
    /// </summary>
    public void SetPosition(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
    }

    /// <summary>
    /// 해당 버튼을 선택합니다.
    /// </summary>
    /// <param name="_button"></param>
    public void SelectButton(int _index)
    {
        //SetPosition()
    }

    /// <summary>
    /// 해당 버튼에 등록된 함수를 실행합니다.
    /// </summary>

    public void ExecuteButton()
    {
        current.button.onClick.Invoke();
    }



    /// <summary>
    /// true = On, false = off로 변경합니다. \n
    /// 버튼이 눌렸을 때 On, 그냥 선택만 되었을 때 Off입니다.
    /// </summary>
    public void SetImageType(bool _b)
    {
        if (_b)
        {
            selectorImage.sprite = images.select_on;
        }
        else
        {
            selectorImage.sprite = images.select_off;
        }
    }

}
