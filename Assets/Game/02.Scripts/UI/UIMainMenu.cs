using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UIBase
{
    private void Start()
    {
        
    }
    public void TestFunc()
    {
        if (SceneChanger.Instance == null)
        {
            Debug.LogWarning("SceneChanger is Null");
        }

        StartCoroutine(SceneChanger.Instance.LoadThisScene_Joke("TestHomeScene"));
    }

    public override void Init()
    {
        base.Init();
    }

    protected override void CheckOpen()
    {
        base.CheckOpen();
    }

    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        return true;
    }

    public override bool Close()
    {
        StartCoroutine(ProcessClose());
        return true;

    }

    public override void RegisterUIManager()
    {
        base.RegisterUIManager();
    }
}
