using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 팝업창
/// </summary>
public class UIPopup : UIBase
{

    [Header("UIPopup")]
    [Tooltip("true일 때, 게임 오브젝트의 이름으로 UIManager의 딕셔너리에 등록합니다.")]
    public bool addDictionary = false;
    private void Start()
    {
        Init();
        if (addDictionary)
        {
            UIManager.Instance.AddDict(gameObject.name, this);
        }
    }

    public override void Init()
    {
        CheckOpen();
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
        StartCoroutine(ProcessClose());
        return true;
        //Com.canvas.enabled = false;
        //return !(isOpen = Com.canvas.enabled);
    }

    public void Button_OpenThis(UIBase _uiBase)
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenThis(_uiBase);
    }
    public void Button_OpenThis(string _uiName)
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenThis(_uiName);
    }
    public void Button_CloseTop()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.CloseTop();
    }

    public void Button_QuitGame()
    {
        UIManager.Instance.PlayAudio_Click();
        UIManager.Instance.OpenQuitPopup();
    }
}
