using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class SelectorController : MonoBehaviour
{

    #region class
    [Serializable]
    public class Images
    {
        [Tooltip("������ ���� �̹���")]
        public Sprite select_on;

        [Tooltip("������ �ʰ� ���ø� �� ������ �̹���")]
        public Sprite select_off;
    }

    [Serializable]
    public class Currents
    {
        [ReadOnly]
        public int index;
        [ReadOnly]
        public SelectorButton selectorButton;
    }
    #endregion

    [SerializeField]
    [Tooltip("Ŀ��")]
    private SelectorCursor cursor;

    [Tooltip("ó������ ���õ� ��ư�� �ε���")]
    public int selectIndexOnStart;

    [Tooltip("�����Ϳ� ����� ��ư��")]
    public SelectorButton[] buttons;

    [SerializeField]
    private Images images;

    public Currents current;

    /// <summary>
    /// ��ϵ� ��ư�� �� �����Դϴ�.
    /// </summary>
    public int buttonsCount { get; private set; }

    [Tooltip("true�� ���, �������� �������� ������Ʈ���� �ʽ��ϴ�.")]
    private bool doNotUpdateSelectorPosition;

    private void Awake()
    {
        buttonsCount = buttons.Length;

        //��Ʈ�ѷ� �Ҵ�
        for (int i = 0; i < buttonsCount; i++)
        {
            buttons[i].SetController(this);
            buttons[i].index = i;
            Button button = buttons[i].button;
            button.onClick.AddListener(() => Button_SetOnImage());
            button.onClick.AddListener(() => Button_SetEventNull());
        }
    }

    private void Start()
    {
        SelectButton(selectIndexOnStart);
    }

    private int AbsCurrentIndex()
    {
        if (current.index < 0)
        {
            return current.index = buttonsCount - 1;
        }
        else
        {
            return current.index;
        }
    }

    public void DetectKey()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteButton();
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            current.index = (current.index - 1) % buttonsCount;
            AbsCurrentIndex();
            SelectButton(current.index);

            return;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            current.index = (current.index + 1) % buttonsCount;
            AbsCurrentIndex();
            //current.index = Mathf.Abs(current.index);
            SelectButton(current.index);

            return;
        }
    }



    /// <summary>
    /// ������ �������� ������Ʈ�մϴ�.  <see cref="DoNotUpdateSelectorPosition"/>���� ������Ʈ�� ���� �ʰ� �� �� �ֽ��ϴ�.
    /// </summary>
    public void DoUpdateSelectorPosition()
    {
        doNotUpdateSelectorPosition = false;
    }

    /// <summary>
    /// ������ �������� ������Ʈ���� �ʽ��ϴ�.  <see cref="DoUpdateSelectorPosition"/>���� ������Ʈ�� �ϰ� �� �� �ֽ��ϴ�.
    /// </summary>
    public void DoNotUpdateSelectorPosition()
    {
        doNotUpdateSelectorPosition = true;
    }
    /// <summary>
    /// �������� ��ġ�� �����մϴ�.
    /// </summary>
    private void SetPosition(Vector2 _pos)
    {
        cursor.SetPosition(_pos);
    }

    /// <summary>
    /// �ش� ��ư�� �����մϴ�.
    /// </summary>
    public void SelectButton(SelectorButton _button)
    {
        if (doNotUpdateSelectorPosition)
        {
            return;
        }
        SetImageType(false);

        cursor.currentIndex = _button.index;
        current.index = _button.index;
        current.selectorButton = _button;

        //��ġ �̵�
        SetPosition(current.selectorButton.rect.anchoredPosition);
    }

    /// <summary>
    /// �ش� ��ư�� �����մϴ�.
    /// </summary>
    public void SelectButton(int _index)
    {
        if (doNotUpdateSelectorPosition)
        {
            return;
        }
        SetImageType(false);

        cursor.currentIndex = _index;
        current.index = _index;
        current.selectorButton = buttons[_index];

        //��ġ �̵�
        SetPosition(current.selectorButton.rect.anchoredPosition);
    }

    /// <summary>
    /// �ش� ��ư�� ��ϵ� �Լ��� �����մϴ�.
    /// </summary>
    public void ExecuteButton()
    {
        Button tempButton = current.selectorButton.button;
        tempButton.onClick.Invoke();
    }

    /// <summary>
    /// ��ư�� ������ �� Ŀ���� �̹����� ��ü�մϴ�.
    /// </summary>
    public void Button_SetOnImage()
    {
        SetImageType(true);
    }
    public void Button_SetEventNull()
    {
        EventSystem currentEvent = EventSystem.current;
        currentEvent.SetSelectedGameObject(null);
    }


    /// <summary>
    /// true = On, false = off�� �����մϴ�. \n
    /// ��ư�� ������ �� On, �׳� ���ø� �Ǿ��� �� Off�Դϴ�.
    /// </summary>
    public void SetImageType(bool _b)
    {
        if (_b)
        {
            cursor.SetSprite(images.select_on);
        }
        else
        {
            cursor.SetSprite(images.select_off);
        }
    }

    //private IEnumerator CoChangeImageType()
    //{
    //    yield return 
    //}
}
