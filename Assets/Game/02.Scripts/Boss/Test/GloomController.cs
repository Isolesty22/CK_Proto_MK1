using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomController : BossController
{
    public override void ChangeState(int _state)
    {
        base.ChangeState(_state);
    }
    public override string GetStateToString(int _state)
    {
        return base.GetStateToString(_state);
    }

    public override void OnAnimStateExit()
    {
        base.OnAnimStateExit();
    }
    protected override void Init()
    {
        base.Init();
    }
}


[Serializable]
public struct GloomPattern
{
    //[Tooltip("실행할 패턴")]
    //public eBearState state;

    //[Tooltip("실행 후 대기 시간")]
    //public float waitTime;
}
