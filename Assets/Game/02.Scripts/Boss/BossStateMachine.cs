using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    [Tooltip("현재 스테이트")]
    public BossState currentState = null;

    [Tooltip("현재 상태 int")]
    protected int currentStateInt;

    [Tooltip("이전 상태 int")]
    protected int prevStateInt;

    [Tooltip("BossState가 들어있는 딕셔너리")]
    protected Dictionary<int, BossState> stateDict = new Dictionary<int, BossState>();

    public bool isDebugMode = false;

    /// <summary>
    /// 아무런 검사를 하지 않고 상태에 진입합니다.
    /// </summary>
    /// <param name="_state"></param>
    public virtual void StartState(int _state)
    {
        BossState tempState = GetState(_state);
        stateDict.Add(_state, tempState);

        prevStateInt = 0;
        currentStateInt = _state;

        //현재 스테이트 변경
        currentState = stateDict[_state];

        //상태 진입
        //currentState.OnEnter();
        //LogWarning(currentStateEnum.ToString() + " - Enter");
    }

    public virtual void ChangeState(int _state)
    {

    }

    public void Update()
    {
        currentState.OnUpdate();
    }

    public void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public virtual BossState GetState(int _state)
    {
        //switch _state...

        return new BossState();
    }

    public string GetCurrentStateName()
    {
        return currentState.ToString();
    }

    public bool CanExit()
    {
        return currentState.canExit;
    }
    #region Log 
    protected void Log(object _message)
    {
        if (isDebugMode)
        {
            Debug.Log("BossStateMachine : " + _message);

        }
    }
    protected void LogWarning(object _message)
    {
        if (isDebugMode)
        {
            Debug.LogWarning("BossStateMachine : " + _message);
        }
    }
    protected void LogError(object _message)
    {
        if (isDebugMode)
        {
            Debug.LogError("BossStateMachine : " + _message);
        }
    }
    #endregion
}
