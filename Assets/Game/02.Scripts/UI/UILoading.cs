using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoading : UIBase
{
    private void Start()
    {
        Init();
        RegisterUIManager();
    }

    public override void Init()
    {
        base.Init();
    }

    public override bool Open()
    {
        return base.Open();
    }
    public override bool Close()
    {
        return base.Close();
    }

    /// <summary>
    /// UIManager�� �ش� UI�� ����մϴ�.
    /// </summary>
    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}
