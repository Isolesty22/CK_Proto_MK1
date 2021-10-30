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
        BossState tempState = null;

        //딕셔너리에 있는 상태라면
        if (stateDict.TryGetValue(_state, out tempState))
        {
            //if (currentState == stateDict[_state])
            //{
            //    LogError("같은 스테이트로는 변경할 수 없습니다.");
            //    return;
            //}
        }
        //딕셔너리에 안들어있었으면
        else
        {
            //스테이트 만들어서 넣기
            tempState = GetState(_state);
            stateDict.Add(_state, tempState);
        }


        //상태 끝내기
        if (!ReferenceEquals(currentState, null))
        {
            currentState.OnExit();
            LogWarning(currentState.ToString() + " - Exit");
        }


        //enum들 설정
        prevStateInt = currentStateInt;
        currentStateInt = _state;

        //현재 스테이트 변경
        currentState = stateDict[_state];

        //상태 진입
        currentState.OnEnter();
        LogWarning(currentState.ToString() + " - Enter");
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

        return null;
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
