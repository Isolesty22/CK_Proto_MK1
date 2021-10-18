using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UI를 작동/관리하는 매니저 클래스
/// </summary>

public class UIManager : MonoBehaviour
{
    [Header("팝업창")]
    public UIPopup_New uiPopup_new;

    [Header("씬 이동 시 파괴하지 않음")]
    public bool dontDestroyOnLoad;

    [Header("OpenThis호출 시 이전 UI가 꺼짐")]
    public bool disabledPrevUI;

    #region Instance
    private static UIManager instance;

    public static UIManager Instance;
    #endregion

    [Tooltip("현재 열려있는 UI 개수")]
    public int openUIcount;
    [Space(10)]

    [SerializeField]
    private Stack<UIBase> uiStack = new Stack<UIBase>();

    [Tooltip("가장 최근에 접근 시도했던 UIBase")]
    private UIBase latelyUI;

    [Tooltip("[임시] 패배 팝업")]
    public UILosePopup losePopup;

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            Instance = instance;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
            if (Instance != this) //나 자신이 인스턴스가 아닐 경우
            {
                Debug.Log(this + " : 더 이상, 이 세계선에서는 존재할 수 없을 것 같아... 안녕.");
                Destroy(this.gameObject);
            }

        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        CloseTop();
    //    }
    //}

    /// <summary>
    /// Stack에서, 현재 UI 이전에 있는 UI
    /// </summary>
    private UIBase prevUI = null;
    public void OpenThis(UIBase _uiBase)
    {
        if (_uiBase.isOpen)
        {
            Debug.Log("이미 열려있어!");
            return;
        }

        latelyUI = _uiBase;

        if (latelyUI.Open()) //성공적으로 열렸으면
        {
            if (uiStack.Count != 0) //만약 다른 UI가 있다면
            {
                //다른 UI를 비활성화
                prevUI = uiStack.Peek();
                prevUI.Com.canvasGroup.interactable = false;

                if (disabledPrevUI)
                {
                    prevUI.SetCanvasEnabled(false);
                }
                //uiStack.Peek().Com.canvasGroup.interactable = false;
                //uiStack.Peek().Com.
            }

            uiStack.Push(latelyUI);

            latelyUI.Com.canvasGroup.interactable = true;
            latelyUI.Com.canvas.sortingOrder = 10 + uiStack.Count;
        }

        openUIcount = uiStack.Count;
    }

    /// <summary>
    /// Stack 맨 위에 있는 UI를 닫습니다.
    /// </summary>
    public void CloseTop()
    {
        if (uiStack.Count == 0)
        {
            Debug.Log("닫을 수 있는 UI가 없어!");
            return;
        }

        latelyUI = uiStack.Peek();

        if (latelyUI.Close()) //성공적으로 닫혔으면
        {
            uiStack.Pop();

            if (uiStack.Count != 0) //만약 다른 UI가 있다면
            {
                //다른 UI를 활성화
                prevUI = uiStack.Peek();
                prevUI.Com.canvasGroup.interactable = true;


                if (disabledPrevUI)
                {
                    //prevUI.Open();
                    prevUI.SetCanvasEnabled(true);
                }

                //uiStack.Peek().Com.canvasGroup.interactable = true;
            }
        }
        openUIcount = uiStack.Count;
    }

    /// <summary>
    /// 팝업창 하나를 열고, 내용, 버튼 이벤트를 초기화합니다.
    /// </summary>
    /// <param name="_left">오른쪽 버튼에 적용되는 이벤트</param>
    /// <param name="_right">왼쪽 버튼에 적용되는 이벤트</param>
    public void OpenPopup(eUIText _uiText, UnityAction _left, UnityAction _right)
    {
        uiPopup_new.Init_Popup(_uiText, _left, _right);
        OpenThis(uiPopup_new);

    }


    /// <summary>
    /// [임시] 패배 팝업을 띄웁니다.
    /// </summary>
    public void OpenLosePopup()
    {
        OpenThis(losePopup);
    }
}
