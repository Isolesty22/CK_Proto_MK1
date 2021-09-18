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

    public override void StartState(eBossState _state)
    {
        base.StartState(_state);
    }
    public override void ChangeState(eBossState _state)
    {
        base.ChangeState(_state);
    }

    public override BossState GetState(eBossState _state)
    {

        switch (_state)
        {
            case eBossState.BearState_Idle:
                return new BearState_Idle(bearController);

            case eBossState.BearState_Doljin:
                return new BearState_Doljin(bearController);

            case eBossState.BearState_Halquigi_A:
                return new BearState_Halquigi_A(bearController);

            case eBossState.BearState_Halquigi_B:
                return new BearState_Halquigi_B(bearController);

            case eBossState.BearState_Phohyo:
                return new BearState_Phohyo(bearController);

            case eBossState.BearState_Naeryeochigi:
                return new BearState_Naeryeochigi(bearController);

            default:
                return new BearState_Idle(bearController);
        }
    }

}
