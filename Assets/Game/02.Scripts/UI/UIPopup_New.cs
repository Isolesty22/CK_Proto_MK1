using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup_New : UIBase
{

    [Header("팝업 관련")]
    public Text message;
    public Button button_yes;
    public Button button_no;

    /// <summary>
    /// 팝업창의 내용, 버튼 이벤트를 초기화합니다.
    /// </summary>
    /// <param name="_text">내용(\n등은 제대로 적용됨)</param>
    /// <param name="_yes">왼쪽 버튼에 적용되는 이벤트</param>
    /// <param name="_no">오른쪽 버튼에 적용되는 이벤트</param>
    public void Init_Popup(string _text, UnityEngine.Events.UnityAction _yes, UnityEngine.Events.UnityAction _no)
    {
        message.text = _text;
        button_yes.onClick.AddListener(_yes);
        button_no.onClick.AddListener(_no);
    }
    private void Start()
    {
        Init();
        RegisterUIManager();
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
        button_yes.onClick.RemoveAllListeners();
        button_no.onClick.RemoveAllListeners();

        StartCoroutine(ProcessClose());
        return true;
        //Com.canvas.enabled = false;
        //return !(isOpen = Com.canvas.enabled);
    }


    public void CloseMe()
    {
        UIManager.Instance.CloseTop();
    }
    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}
