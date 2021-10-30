using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomController : BossController
{
    [Header("이동 시 사용하는 강체")]
    [Tooltip("글룸은 이동할 때 트랜스폼을 사용하지 않고, \n리지드바디를 사용합니다.")]
    public Rigidbody myRigidbody;

    private void Start()
    {
        Init();
    }
    protected override void Init()
    {
        base.Init();

        stateMachine = new GloomStateMachine(this);
        stateMachine.StartState((int)eGloomState.Idle);
    }




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
}


[Serializable]
public struct GloomPattern
{
    //[Tooltip("실행할 패턴")]
    //public eBearState state;

    //[Tooltip("실행 후 대기 시간")]
    //public float waitTime;
}
