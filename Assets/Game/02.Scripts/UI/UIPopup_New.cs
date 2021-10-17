using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIPopup_New : UIBase
{
    public GameObject[] textImages;
    public Dictionary<eUIText, GameObject> textImageDict = new Dictionary<eUIText, GameObject>();

    [HideInInspector]
    public GameObject currentTextImage = null;
    [Header("팝업 관련")]
    public Button button_left;
    public Button button_right;

    /// <summary>
    /// 팝업창의 내용, 버튼 이벤트를 초기화합니다.
    /// </summary>
    /// <param name="_text">내용(\n등은 제대로 적용됨)</param>
    /// <param name="_yes">왼쪽 버튼에 적용되는 이벤트</param>
    /// <param name="_no">오른쪽 버튼에 적용되는 이벤트</param>
    public void Init_Popup(eUIText _uiText, UnityAction _left, UnityAction _right)
    {
        if (currentTextImage != null)
        {
            currentTextImage.SetActive(false);
        }

        textImageDict.TryGetValue(_uiText, out currentTextImage);

        if (currentTextImage != null)
        {
            currentTextImage.SetActive(true);
        }
        button_left.onClick.AddListener(_left);
        button_right.onClick.AddListener(_right);
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        return true;

        //Com.canvas.enabled = true;
        //return isOpen = Com.canvas.enabled;
    }

    public override bool Close()
    {

        //RemoveListeners
        button_right.onClick.RemoveAllListeners();
        button_left.onClick.RemoveAllListeners();

        StartCoroutine(ProcessClose());
        return true;
        //Com.canvas.enabled = false;
        //return !(isOpen = Com.canvas.enabled);
    }


    public void CloseMe()
    {
        UIManager.Instance.CloseTop();
    }
}
