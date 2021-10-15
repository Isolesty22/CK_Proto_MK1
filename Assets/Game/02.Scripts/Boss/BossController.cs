using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossController : MonoBehaviour
{
    public Animator animator;
    public Transform myTransform;

    [Tooltip("애니메이션 이벤트를 위해 사용합니다." +
        "\n애니메이터 컴포넌트가 있는 오브젝트와 동일한 오브젝트에 있어야 합니다.")]
    public AnimationEventListener animationEventListener;

    [Tooltip("애니메이터 파라미터 딕셔너리")]
    public Dictionary<string, int> aniHash = new Dictionary<string, int>();

    protected readonly string str_SkillVarietyBlend = "SkillVarietyBlend";
    protected int skillVarietyBlend = 0;


    [Tooltip("패턴 전환 코루틴")]
    protected IEnumerator ExecutePatternCoroutine;


    #region Animation 관련

    public virtual void OnAnimStateExit() { }

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
    public float phase2;
    public float phase3;
}