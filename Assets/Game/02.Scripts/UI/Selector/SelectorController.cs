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
        [Tooltip("눌렀을 때의 이미지")]
        public Sprite select_on;

        [Tooltip("누르지 않고 선택만 된 상태의 이미지")]
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
    [Tooltip("커서")]
    private SelectorCursor cursor;

    [Tooltip("처음부터 선택될 버튼의 인덱스")]
    public int selectIndexOnStart;

    [Tooltip("셀렉터에 등록할 버튼들")]
    public SelectorButton[] buttons;

    [SerializeField]
    private Images images;

    public Currents current;

    /// <summary>
    /// 등록된 버튼의 총 개수입니다.
    /// </summary>
    public int buttonsCount { get; private set; }

    [Tooltip("true일 경우, 셀렉터의 포지션을 업데이트하지 않습니다.")]
    private bool doNotUpdateSelectorPosition;

    private void Awake()
    {
        buttonsCount = buttons.Length;

        //컨트롤러 할당
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
    /// 셀렉터 포지션을 업데이트합니다.  <see cref="DoNotUpdateSelectorPosition"/>으로 업데이트를 하지 않게 할 수 있습니다.
    /// </summary>
    public void DoUpdateSelectorPosition()
    {
        doNotUpdateSelectorPosition = false;
    }

    /// <summary>
    /// 셀렉터 포지션을 업데이트하지 않습니다.  <see cref="DoUpdateSelectorPosition"/>으로 업데이트를 하게 할 수 있습니다.
    /// </summary>
    public void DoNotUpdateSelectorPosition()
    {
        doNotUpdateSelectorPosition = true;
    }
    /// <summary>
    /// 셀렉터의 위치를 변경합니다.
    /// </summary>
    private void SetPosition(Vector2 _pos)
    {
        cursor.SetPosition(_pos);
    }

    /// <summary>
    /// 해당 버튼을 선택합니다.
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

        //위치 이동
        SetPosition(current.selectorButton.rect.anchoredPosition);
    }

    /// <summary>
    /// 해당 버튼을 선택합니다.
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

        //위치 이동
        SetPosition(current.selectorButton.rect.anchoredPosition);
    }

    /// <summary>
    /// 해당 버튼에 등록된 함수를 실행합니다.
    /// </summary>
    public void ExecuteButton()
    {
        Button tempButton = current.selectorButton.button;
        tempButton.onClick.Invoke();
    }

    /// <summary>
    /// 버튼을 눌렀을 때 커서의 이미지만 교체합니다.
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
    /// true = On, false = off로 변경합니다. \n
    /// 버튼이 눌렸을 때 On, 그냥 선택만 되었을 때 Off입니다.
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
