using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageStart : UIBase
{
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
    }
    public override bool Open()
    {
        StartCoroutine(ProcessOpen());
        Time.timeScale = 0f;
        return true;
    }

    public override bool Close()
    {
        StartCoroutine(ProcessClose());
        Time.timeScale = 1f;
        return true;
    }

    protected override IEnumerator ProcessClose()
    {
        return base.ProcessClose();
    }

    protected override IEnumerator ProcessOpen()
    {
        return base.ProcessOpen();
    }
}
