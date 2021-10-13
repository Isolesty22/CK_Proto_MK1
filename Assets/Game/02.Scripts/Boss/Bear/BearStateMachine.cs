using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStateMachine : BossStateMachine
{
    private BearController bearController;
    public BearStateMachine(BearController _bearController)
    {
        bearController = _bearController;
    }

    public override void StartState(int _state)
    {
        base.StartState(_state);
    }
    public override void ChangeState(int _state)
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
            LogWarning(currentStateInt.ToString() + " - Exit");
        }


        //enum들 설정
        prevStateInt = currentStateInt;
        currentStateInt = _state;

        //현재 스테이트 변경
        currentState = stateDict[_state];

        //상태 진입
        currentState.OnEnter();
        LogWarning(currentStateInt.ToString() + " - Enter");
    }
    public override BossState GetState(int _state)
    {
        eBossState tempState = (eBossState)_state;

        switch (tempState)
        {
            case eBossState.BearState_Idle:
                return new BearState_Idle(bearController);

            case eBossState.BearState_Stamp:
                return new BearState_Stamp(bearController);



            case eBossState.BearState_Roar_A:
            case eBossState.BearState_Roar_B:
                return new BearState_Roar(bearController);

            case eBossState.BearState_Strike_A:
            case eBossState.BearState_Strike_B:
            case eBossState.BearState_Strike_C:
                return new BearState_Strike(bearController);

            case eBossState.BearState_Claw_A:
            case eBossState.BearState_Claw_B:
            case eBossState.BearState_Claw_C:
                return new BearState_Claw(bearController);

            case eBossState.BearState_Smash:
                return new BearState_Smash(bearController);

            case eBossState.BearState_Concentrate:
                return new BearState_Concentrate(bearController);

            case eBossState.BearState_Powerless:
                return new BearState_Powerless(bearController);

            case eBossState.BearState_Rush:
                return new BearState_Rush(bearController);
           
            case eBossState.BearState_FinalWalk:
                return new BearState_FinalWalk(bearController);

            case eBossState.BearState_Die:
                return new BearState_Die(bearController);

            default:
                return new BearState_Idle(bearController);
        }
    }
}
