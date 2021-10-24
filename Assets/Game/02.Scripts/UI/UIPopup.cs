using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÆË¾÷Ã¢
/// </summary>
public class UIPopup : UIBase
{
    private void Start()
    {
        Init();
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
        UIManager.Instance.OpenThis(_uiBase);
    }
    public void Button_CloseTop()
    {
        UIManager.Instance.CloseTop();
    }
}
