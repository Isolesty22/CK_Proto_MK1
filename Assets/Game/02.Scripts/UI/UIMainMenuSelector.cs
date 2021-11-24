using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIMainMenuSelector : MonoBehaviour
{

    [Tooltip("�����Ϳ� ����� ��ư��")]
    public Button[] buttons;

    [Tooltip("��ư���� ��Ʈ Ʈ������")]
    private RectTransform[] buttonRects;

    public int selectIndexOnStart;

    [Serializable]
    public class Images
    {
        [Tooltip("������ ���� �̹���")]
        public Sprite select_on;

        [Tooltip("������ �ʰ� ���ø� �� ������ �̹���")]
        public Sprite select_off;
    }

    [SerializeField]
    [Tooltip("�������� �̹����Դϴ�.")]
    private Image selectorImage;

    [SerializeField]
    [Tooltip("�������� ��Ʈ Ʈ�������Դϴ�.")]
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
    /// ��ϵ� ��ư�� �� �����Դϴ�.
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
    /// �������� ��ġ�� �����մϴ�.
    /// </summary>
    public void SetPosition(Vector2 _pos)
    {
        rect.anchoredPosition = _pos;
    }

    /// <summary>
    /// �ش� ��ư�� �����մϴ�.
    /// </summary>
    /// <param name="_button"></param>
    public void SelectButton(int _index)
    {
        //SetPosition()
    }

    /// <summary>
    /// �ش� ��ư�� ��ϵ� �Լ��� �����մϴ�.
    /// </summary>

    public void ExecuteButton()
    {
        current.button.onClick.Invoke();
    }



    /// <summary>
    /// true = On, false = off�� �����մϴ�. \n
    /// ��ư�� ������ �� On, �׳� ���ø� �Ǿ��� �� Off�Դϴ�.
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
