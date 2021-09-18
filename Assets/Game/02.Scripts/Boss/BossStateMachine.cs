using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{

    [Tooltip("현재 스테이트")]
    protected BossState currentState = null;

    [Tooltip("현재 상태 enum")]
    protected eBossState currentStateEnum;

    [Tooltip("이전 상태 enum")]
    protected eBossState prevStateEnum;

    [Tooltip("BossState가 들어있는 딕셔너리")]
    protected Dictionary<eBossState, BossState> stateDict = new Dictionary<eBossState, BossState>();

    public virtual void ChangeState(eBossState _state)
    {
        BossState tempState = null;

        //딕셔너리에 있는 상태라면
        if (stateDict.TryGetValue(_state, out tempState))
        {
            if (currentState == stateDict[_state])
            {
                LogError("같은 스테이트로는 변경할 수 없습니다.");
                return;
            }
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
        }


        //enum들 설정
        prevStateEnum = currentStateEnum;
        currentStateEnum = _state;

        //현재 스테이트 변경
        currentState = stateDict[_state];

        //상태 진입
        currentState.OnEnter();

    }

    public void Update()
    {
        currentState.OnUpdate();
    }

    public void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public virtual BossState GetState(eBossState _state)
    {
        //switch _state...

        return new BossState();
    }

    public string GetCurrentStateName()
    {
        return currentState.ToString();
    }

    #region Log 
    protected void Log(object _message)
    {
        Debug.Log("BossStateMachine : " + _message);
    }
    protected void LogWarning(object _message)
    {
        Debug.LogWarning("BossStateMachine : " + _message);
    }
    protected void LogError(object _message)
    {
        Debug.LogError("BossStateMachine : " + _message);
    }
    #endregion
}
