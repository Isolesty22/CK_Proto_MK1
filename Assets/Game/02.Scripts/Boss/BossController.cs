using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossController : MonoBehaviour
{
    public Transform myTransform;

    public Animator animator;

    [Tooltip("애니메이션 이벤트를 위해 사용합니다." +
        "\n애니메이터 컴포넌트가 있는 오브젝트와 동일한 오브젝트에 있어야 합니다.")]
    public AnimationEventListener animationEventListener;

    public BossStateMachine stateMachine;

    [Space(10)]
    [Tooltip("현재 상태")]
    public StateInfo stateInfo = new StateInfo();


    [Tooltip("애니메이터 파라미터 딕셔너리")]
    public Dictionary<string, int> aniHash = new Dictionary<string, int>();

    protected readonly string str_SkillVarietyBlend = "SkillVarietyBlend";
    protected int skillVarietyBlend = 0;

    [Tooltip("패턴 전환 코루틴")]
    protected IEnumerator ExecutePatternCoroutine;

    protected virtual void Init() { }

    #region State 관련
    public virtual void ChangeState(int _state)
    {
        SetStateInfo((int)_state);
        stateMachine.ChangeState((int)_state);
    }

    public void SetStateInfo(int _state)
    {
        stateInfo.stateInt = _state;
        stateInfo.state = GetStateToString(_state);
    }
    /// <summary>
    /// 현재 상태의 canExit를 설정합니다.
    /// </summary>
    public void SetCanExit(bool _canExit) => stateMachine.currentState.canExit = _canExit;

    public virtual string GetStateToString(int _state) { return string.Empty; }

    #endregion

    #region Animation 관련
    public void SetSkillVariety(float _v)
    {
        animator.SetFloat(skillVarietyBlend, _v);
    }
    public void SetTrigger(string _paramName)
    {
        animator.SetTrigger(aniHash[_paramName]);
    }
    public virtual void OnAnimStateExit()
    {
        stateMachine.currentState.canExit = true;
    }

    protected void AddAnimatorHash(string _paramName)
    {
        aniHash.Add(_paramName, Animator.StringToHash(_paramName));
    }
    public void SetAnimEvent(Action _event)
    {
        //skillAction = null;
        //skillAction += () => Debug.Log("SkillAction!");
        //skillAction += _action;
        animationEventListener.SetEvent(_event);
    }

    public void AddAnimEvent(Action _event)
    {
        animationEventListener.AddEvent(_event);
    }

    public void CallAnimEvent()
    {
        animationEventListener.CallEvent();
    }
    #endregion
}


[Serializable]
public class StateInfo
{
    [Tooltip("현재 페이즈")]
    public ePhase phase;

    [Tooltip("현재 상태")]
    public string state = "";

    [Tooltip("현재 상태 Int")]
    public int stateInt;

    public static implicit operator int(StateInfo _si) => (int)_si.phase;
}

[Serializable]
public class BossPhaseValue
{
    //[Range(0,1)]
    //public float phase1Per;
    [Range(0,1)]
    public float phase2Per;
    [Range(0,1)]
    public float phase3Per;

    //[HideInInspector]
    //public float phase1Hp;
    [ReadOnly]
    public float phase2Hp;
    [ReadOnly]
    public float phase3Hp;

    public void Init(float _maxHP)
    {
        phase2Hp = _maxHP * phase2Per;
        phase3Hp = _maxHP * phase3Per;
    }
}