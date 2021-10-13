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
        eBearState tempState = (eBearState)_state;

        switch (tempState)
        {
            case eBearState.Idle:
                return new BearState_Idle(bearController);

            case eBearState.Stamp:
                return new BearState_Stamp(bearController);



            case eBearState.Roar_A:
            case eBearState.Roar_B:
                return new BearState_Roar(bearController);

            case eBearState.Strike_A:
            case eBearState.Strike_B:
            case eBearState.Strike_C:
                return new BearState_Strike(bearController);

            case eBearState.Claw_A:
            case eBearState.Claw_B:
            case eBearState.Claw_C:
                return new BearState_Claw(bearController);

            case eBearState.Smash:
                return new BearState_Smash(bearController);

            case eBearState.Concentrate:
                return new BearState_Concentrate(bearController);

            case eBearState.Powerless:
                return new BearState_Powerless(bearController);

            case eBearState.Rush:
                return new BearState_Rush(bearController);
           
            case eBearState.FinalWalk:
                return new BearState_FinalWalk(bearController);

            case eBearState.Die:
                return new BearState_Die(bearController);

            default:
                return new BearState_Idle(bearController);
        }
    }
}
