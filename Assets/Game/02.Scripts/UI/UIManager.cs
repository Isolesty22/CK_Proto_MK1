using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// UI를 작동/관리하는 매니저 클래스
/// </summary>

public class UIManager : MonoBehaviour
{
    #region Instance
    private static UIManager instance;

    public static UIManager Instance;
    #endregion

    [Header("팝업창")]
    public UIPopup_New uiPopup_new;

    [Header("대화창(UITalk)")]
    public UITalk uiTalk;

    [Header("씬 이동 시 파괴하지 않음")]
    public bool dontDestroyOnLoad;

    [Header("OpenThis호출 시 이전 UI가 꺼짐")]
    public bool disabledPrevUI;

    [Header("현재 열려있는 UI 개수"), ReadOnly]
    public int openUIcount;

    [Space(10)]

    [SerializeField]
    private Stack<UIBase> uiStack = new Stack<UIBase>();

    [Tooltip("가장 최근에 접근 시도했던 UIBase")]
    private UIBase latelyUI;

    private Action detectingCloseKey;

    /// <summary>
    /// 이름있는 UIBase들이 들어있을 수 있는 딕셔너리
    /// </summary>
    private Dictionary<string, UIBase> uiDict = new Dictionary<string, UIBase>();

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


        StartDetectingCloseKey();
    }
    private void Update()
    {
        detectingCloseKey();
    }

    /// <summary>
    /// Stack에서, 현재 UI 이전에 있는 UI
    /// </summary>
    private UIBase prevUI = null;

    public void OpenThis(string _name)
    {
        OpenThis(uiDict[_name]);
    }
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
    /// 닫기 키를 감지합니다. 닫기 키를 누르면 CloseTop이 호출됩니다.
    /// </summary>
    public void StartDetectingCloseKey()
    {
        detectingCloseKey = DetectingCloseKey;
    }

    /// <summary>
    /// 닫기 키 감지를 멈춥니다.
    /// </summary>
    public void StopDetectingCloseKey()
    {
        detectingCloseKey = VoidFunc;
    }

    private void DetectingCloseKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openUIcount == 0)
            {
                //일시정지 UI가 있다면
                if (uiDict.ContainsKey(UIName.UIPause))
                {
                    OpenThis(uiDict[UIName.UIPause]);
                }
                //없으면 종료팝업 띄우기 ( 아마 메인메뉴일 것 같은데...?)
                else
                {
                    OpenQuitPopup();
                }

            }
            else
            {
                CloseTop();
            }

        }
    }

    /// <summary>
    /// [주의] 씬에 오직 하나만 존재할 수 있는 UI에만 사용하세요.
    /// </summary>
    /// <param name="_uiBase"></param>
    public void AddDict(UIBase _uiBase)
    {
        uiDict.Add(_uiBase.GetType().Name, _uiBase);
        //Debug.Log(_uiBase.GetType().Name);
    }
    public void AddDict(string _name, UIBase _uiBase) => uiDict.Add(_name, _uiBase);
    public void RemoveDict(string _name) => uiDict.Remove(_name);

    /// <summary>
    /// uiDict에서 UIBase를 반환합니다(오류검사를 하지 않습니다). 
    /// </summary>
    public UIBase GetUI(string _name) => uiDict[_name];

    /// <summary>
    /// 게임 종료 팝업을 엽니다.
    /// </summary>
    public void OpenQuitPopup()
    {
        OpenPopup(eUIText.Exit,
            QuitGame,
            CloseTop);
    }

    /// <summary>
    /// 지속시간에 영향을 받지 않고, 닫기 전까지 열려있는 대화를 시작합니다. <see cref="TalkEnd"/>를 호출하여 창을 닫을 수 있습니다.
    /// </summary>
    public void TalkInfinity(int _CODE)
    {
        uiTalk.SetValue(_CODE);
        uiTalk.StartTalkInfinity();
    }
    /// <summary>
    /// 대화창을 닫습니다. 대화창이 열려있는 상태에서만 작동합니다.
    /// </summary>
    public void TalkEnd()
    {
        uiTalk.EndTalk();
    }

    /// <summary>
    /// 지속시간대로 대화를 시작합니다. 지속시간이 지나면 자동으로 닫힙니다.
    /// </summary>
    public void Talk(int _CODE, float _duration)
    {
        uiTalk.SetValue(_CODE, _duration);
        uiTalk.StartTalk();
    }

    /// <summary>
    /// 지속시간을 신경쓰지않고 대화를 시작합니다. 이전 대화의 지속시간을 따릅니다.
    /// </summary>
    public void Talk(int _CODE)
    {
        uiTalk.SetValue(_CODE);
        uiTalk.StartTalk();
    }

    public void Talk(string _str)
    {
        uiTalk.SetText_Test(_str);
        //uiTalk.StartTalkInfinity();
        uiTalk.StartTalk();
    }

    /// <summary>
    /// 게임을 종료합니다.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    private void VoidFunc() { }
}