using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �˾�â
/// </summary>
public class UIPopup : UIBase
{
    private void Start()
    {
        Init();
        RegisterUIManager();
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
    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}
