using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI를 작동/관리하는 매니저 클래스
/// </summary>

public class UIManager : MonoBehaviour
{
    [Header("씬 이동 시 파괴하지 않음")]
    public bool dontDestroyOnLoad;

    #region Instance
    private static UIManager instance;

    public static UIManager Instance;
    #endregion

    [Space(10)]

    [SerializeField]
    private List<UIBase> uiList = new List<UIBase>();

    [Tooltip("하나 밖에 존재하지 않는 UI들을 위한 딕셔너리")]
    public Dictionary<string, UIBase> uiDict = new Dictionary<string, UIBase>();

    [SerializeField]
    private Stack<UIBase> uiStack = new Stack<UIBase>();

    [Tooltip("가장 최근에 접근 시도했던 UIBase")]
    private UIBase latelyUI;

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
                //uiStack.Peek().Com.canvasGroup.interactable = false;
            }

            uiStack.Push(latelyUI);

            //latelyUI.Com.canvasGroup.interactable = true;
            latelyUI.Com.canvas.sortingOrder = uiStack.Count;
        }
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
                // uiStack.Peek().Com.canvasGroup.interactable = true;
            }
        }

    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    /// <summary>
    /// uiList에 해당 uiBase를 Add합니다.
    /// </summary>
    public void RegisterListThis(UIBase _uiBase)
    {
        uiList.Add(_uiBase);
        //Debug.Log("uiList에 " + _uiBase.name + " 추가!");
    }

    public void RegisterDictThis(string _uiName ,UIBase _uiBase)
    {
        uiDict.Add(_uiName, _uiBase);
        //Debug.Log("uiList에 " + _uiBase.name + " 추가!");
    }
}
