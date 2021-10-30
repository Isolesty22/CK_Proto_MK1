using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomController : BossController
{
    [Header("이동 시 사용하는 강체")]
    [Tooltip("글룸은 이동할 때 트랜스폼을 사용하지 않고, \n리지드바디를 사용합니다.")]
    public Rigidbody myRigidbody;

    [Header("페이즈가 전환되는 HP")]
    public BossPhaseValue bossPhaseValue;
    private void Start()
    {
        Init();
        Init_Animator();
    }
    protected override void Init()
    {
        base.Init();
        bossPhaseValue.Init(hp);
        stateMachine = new GloomStateMachine(this);
        stateMachine.StartState((int)eGloomState.Idle);
    }
    private void Init_Animator()
    {
        GloomStateMachineBehaviour[] behaviours = animator.GetBehaviours<GloomStateMachineBehaviour>();

        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].gloomController = this;
        }

        int paramCount = animator.parameterCount;
        AnimatorControllerParameter[] aniParam = animator.parameters;

        for (int i = 0; i < paramCount; i++)
        {
            AddAnimatorHash(aniParam[i].name);
        }

        skillVarietyBlend = aniHash[str_SkillVarietyBlend];

    }



    public override void ChangeState(int _state)
    {
        base.ChangeState(_state);
    }
    public override string GetStateToString(int _state)
    {
        return base.GetStateToString(_state);
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
