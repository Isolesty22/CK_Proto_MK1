//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;

public class BossController : MonoBehaviour
{
    protected Action skillAction = null;
    public void SetSkillAction(Action _action)
    {
        //skillAction = null;
        //skillAction += () => Debug.Log("SkillAction!");
        //skillAction += _action;
        skillAction = _action;
    }

    public void AddSkillAction(Action _action)
    {
        skillAction += _action;
    }

    public void SkillAction()
    {
        skillAction();
    }
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