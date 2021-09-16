using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageStart : UIBase
{
    private void Awake()
    {
        Init();
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

    protected override IEnumerator ProcessClose()
    {
        return base.ProcessClose();
    }

    protected override IEnumerator ProcessOpen()
    {
        return base.ProcessOpen();
    }
    public void SetFadeDuration(float _value)
    {
         fadeDuration = _value;
    }
    public float GetFadeDuration()
    {
        return fadeDuration;
    }
}
