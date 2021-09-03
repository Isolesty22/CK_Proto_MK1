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
        Com.canvas.enabled = true;
        return isOpen = Com.canvas.enabled;
    }

    public override bool Close()
    {
        Com.canvas.enabled = false;
        return isOpen = Com.canvas.enabled;
    }

    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}
